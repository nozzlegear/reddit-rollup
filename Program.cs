using System;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace reddit_rollup
{
    class Program
    {
        static string GetAppDirectory()
        {
            return System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        }

        static void Main(string[] args)
        {
            var cli = new CommandLineApplication();
            cli.FullName = "Reddit Rollup";

            cli.Argument("--now", "Sends the roundup email immediately rather than waiting for the scheduled time.");

            cli.OnExecute(async () =>
            {
                Console.WriteLine("Arguments: %A", cli.Arguments);
                
                var client = new Reddit();
                var token = await client.GetAccessToken();
                var subs = new string[] { 
                    "politicaldiscussion", 
                    "wholesomememes", 
                    "asmr",
                    "birbs", 
                    "thecompletionist", 
                    "warcraftlore", 
                    "halostory", 
                    "csharp", 
                    "dotnet", 
                    "typescript", 
                    "javascript",
                    "anxiety", 
                };
                string html = string.Empty;

                foreach (var sub in subs)
                {
                    var posts = await client.GetTopPostsForSubreddit(token, sub);

                    foreach (var post in posts)
                    {
                        Console.WriteLine(post.title);

                        html += client.GetPostHtml(post);
                    }
                }

                var path = System.IO.Path.Combine(GetAppDirectory(), "output.html");

                using (var file = System.IO.File.OpenWrite(path))
                {
                    var bytes = System.Text.Encoding.UTF8.GetBytes(html);
                    await file.WriteAsync(bytes, 0, bytes.Length);
                }

                return 0;
            });
            cli.Execute(args);
        }
    }
}
