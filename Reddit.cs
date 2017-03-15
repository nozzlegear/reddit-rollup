using Flurl.Http;
using System;
using System.Threading.Tasks;
using reddit_rollup.Models;
using System.Linq;
using System.Collections.Generic;

namespace reddit_rollup
{
    class Reddit
    {
        private string SWU_KEY { get; set; }

        private string SWU_TEMPLATE_ID { get; set; }

        private const string BASE_PATH = "https://www.reddit.com/";

        public Reddit()
        {
            string keyVarName = "REDDIT_ROLLUP_SWU_KEY";
            string templateVarName = "REDDIT_ROLLUP_SWU_TEMPLATE_ID";

            SWU_KEY = Environment.GetEnvironmentVariable(keyVarName);
            SWU_TEMPLATE_ID = Environment.GetEnvironmentVariable(templateVarName);

            if (string.IsNullOrEmpty(SWU_KEY))
            {
                throw new Exception($"{keyVarName} was not found in environment variables.");
            }
            else if (string.IsNullOrEmpty(SWU_TEMPLATE_ID))
            {
                throw new Exception($"{SWU_TEMPLATE_ID} was not found in environment variables.");
            }
        }

        public async Task<IEnumerable<PostData>> GetTopPostsForSubreddit(string subreddit, int count = 3) 
        {
            using (var client = Flurl.Url.Combine(BASE_PATH, $"r/{subreddit}/top.json?sort=top&t=day").AllowAnyHttpStatus())
            {
                var request = client.GetAsync();
                var result = await request;

                result.EnsureSuccessStatusCode();

                var body = await request.ReceiveJson<SubredditListResponse>();

                return body.data.children.Take(3).Select(p => p.data);
            }
        }

        public string GetPostHtml(PostData post)
        {
            string body = System.Net.WebUtility.HtmlDecode(post.selftext_html);

            if (post.preview != null && post.preview.images?.Count() > 0)
            {
                var preview = post.preview.images.First();
                var image = preview.resolutions.FirstOrDefault(i => i.height < 700 && i.height > 300) ?? preview.source;

                body = $"<a href='{post.url}' target='_blank'><img alt='{post.title}' src='{image.url}' /></a>";
            }
            else if (! string.IsNullOrEmpty(post.thumbnail) && post.thumbnail != "self")
            {
                body = $"<a href='{post.url}' target='_blank'><img alt='{post.title}' src='{post.thumbnail}' /></a>";
            }

            var s = $@"
                <div class='post'>
                    <p>
                        <a href='https://m.reddit.com/{post.subreddit_name_prefixed}' target='_blank'>
                            <strong>{post.subreddit_name_prefixed}:</strong>
                        </a>
                        <a href='https://m.reddit.com/{post.permalink}'>
                            {post.title}
                        </a>
                    </p>
                    {body}
                    <hr />
                </div>
            ";

            return s;
        }

        public async Task<SendWithUsResponse> SendEmail(string html, string subject)
        {
            var request = "https://api.sendwithus.com/api/v1/send"
                .AllowAnyHttpStatus()
                .WithHeader("X-SWU-API-KEY", SWU_KEY)
                .PostJsonAsync(new SendWithUsData()
                {
                    EmailId = SWU_TEMPLATE_ID,
                    Recipient = new Email()
                    {
                        Name = "Joshua Harms",
                        Address = "nozzlegear@outlook.com",
                    },
                    Sender = new Email()
                    {
                        Name = "Reddit Rollup",
                        Address = "reddit-rollup@nozzlegear.com",
                        ReplyTo = "reddit-rollup@nozzlegear.com",
                    },
                    EmailData = new RollupEmailData(html, subject),
                });
            var result = await request;

            try 
            {
                result.EnsureSuccessStatusCode();
            } catch (Exception e)
            {
                var output = new SendWithUsResponse()
                {   
                    Status = result.StatusCode.ToString(),
                    Success = false,
                    ErrorMessage = $"SendWithUs API request failed with {result.StatusCode} {result.ReasonPhrase}. {e.Message}",
                };

                return output;
            }

            return await request.ReceiveJson<SendWithUsResponse>();
        }
    }
}