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
            var filePath = System.IO.Path.Combine(GetAppDirectory(), "output.html");

            var cli = new CommandLineApplication(false);
            cli.FullName = "Reddit Rollup";
            cli.Description = "A command line task that pulls in the top 3 daily posts for a list of subreddits and emails them to a recipient at 0600 CST every day.";

            // Options
            var testOption = cli.Option("--test", $"Skips the email and writes the rollup HTML to {filePath}.", CommandOptionType.NoValue);

            cli.VersionOption("-v | --version", "1.0", "1.0.0");
            cli.HelpOption("-? | -h | --help");
            cli.OnExecute(async () =>
            {  
                var client = new Reddit();
                var subs = new string[] { 
                    "politicaldiscussion", 
                    "wholesomememes", 
                    "asmr",
                    "prequelmemes",
                    "thecompletionist", 
                    "warcraftlore", 
                    "halostory", 
                    "csharp", 
                    "dotnet", 
                    "typescript", 
                    "javascript",
                    "anxiety", 
                };
                string subject = $"Daily Reddit Rollup for {DateTime.UtcNow.ToString("MMM dd, yyyy")}.";
                string html = $"<h1>{subject}</h1><p>Showing the top 3 posts for the last 24 hours.</p><hr/>";

                foreach (var sub in subs)
                {
                    var posts = await client.GetTopPostsForSubreddit(sub);

                    foreach (var post in posts)
                    {
                        html += client.GetPostHtml(post);
                    }
                }

                if (! testOption.HasValue())
                {
                    var response = await client.SendEmail(html, subject);

                    if (!response.Success)
                    {
                        Console.WriteLine(response.ErrorMessage);
                    }
                    else 
                    {
                        Console.WriteLine($"Sent Reddit Roundup email at {DateTime.UtcNow.ToString()} UTC.");
                    }
                }
                else
                {
                    var path = System.IO.Path.Combine(GetAppDirectory(), "output.html");

                    using (var file = System.IO.File.OpenWrite(path))
                    {
                        var bytes = System.Text.Encoding.UTF8.GetBytes(html);
                        await file.WriteAsync(bytes, 0, bytes.Length);
                    }

                    Console.WriteLine("Skipped email and wrote output to {0}.", filePath);
                }

                return 0;
            });
            cli.Execute(args);
        }
    }
}
