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
        private string CLIENT_SECRET { get; set; }

        private string CLIENT_ID { get; set; }

        private string USERNAME { get; set; }

        private string PASSWORD { get; set; }

        private const string BASE_PATH = "https://www.reddit.com/";

        public Reddit()
        {
            CLIENT_SECRET = Environment.GetEnvironmentVariable("REDDIT_ROLLUP_SECRET");
            CLIENT_ID = Environment.GetEnvironmentVariable("REDDIT_ROLLUP_CLIENT_ID");
            USERNAME = Environment.GetEnvironmentVariable("REDDIT_ROLLUP_USERNAME");
            PASSWORD = Environment.GetEnvironmentVariable("REDDIT_ROLLUP_PASSWORD");
        }

        private IFlurlClient ConfigureRequest(string path, string accessToken = null)
        {
            IFlurlClient client = new FlurlClient().AllowAnyHttpStatus();

            if (! string.IsNullOrEmpty(accessToken))
            {
                client = client.WithOAuthBearerToken(accessToken);
            }
            else 
            {
                client = client.WithBasicAuth(CLIENT_ID, CLIENT_SECRET);
            }

            return Flurl.Url.Combine(BASE_PATH, path).WithClient(client);
        }

        public async Task<string> GetAccessToken()
        {
            // 1. Make a request to https://www.reddit.com/api/v1/access_token?grant_type=client_credentials
            // 2. Use basic auth to send client_id as the username and client_secret as the password.
            // 3. Receive a response with the following JSON:
            // {
            //     "access_token": Your access token,
            //     "token_type": "bearer",
            //     "expires_in": Unix Epoch Seconds,
            //     "scope": A scope string,
            // }
            // 4. This type of access token never receives a refresh token. Not sure if that means they never expire, or you just can't refresh and must create another.

            using (var client = ConfigureRequest("api/v1/access_token"))
            {
                var request = client.PostUrlEncodedAsync(new
                {
                    grant_type = "password",
                    username= USERNAME,
                    password = PASSWORD,
                });
                var result = await request;

                result.EnsureSuccessStatusCode();

                var body = await request.ReceiveJson<AccessTokenGrantResponse>();
                
                Console.WriteLine($"Received access token result. Token: {body.access_token}. Expires: {body.expires_in}.");

                return body.access_token;
            }
        }

        public async Task<IEnumerable<PostData>> GetTopPostsForSubreddit(string token, string subreddit, int count = 3) 
        {
            using (var client = ConfigureRequest($"r/{subreddit}/top.json?sort=top&t=day"))
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
            var s = $@"
                <div class='post'>
                    <p>
                        <a href='https://www.reddit.com/{post.subreddit_name_prefixed}' target='_blank'>
                            <strong>{post.subreddit_name_prefixed}:</strong>
                        </a>
                        <a href='{post.url}'>
                            {post.title}
                        </a>
                    </p>
                    {System.Net.WebUtility.HtmlDecode(post.selftext_html)}
                    <hr />
                </div>
            ";

            return s;
        }
    }
}