# Reddit Rollup

In an effort to break my semi-addiction to Reddit, I've created this small command line utility. The goal is to go out and get the top 3 posts from a list of subreddits every morning and email them to myself. Together with making an effort to *only* use Reddit via this daily email, I'm aiming to cut the habit of wasting half my day browsing stories and telling myself that it's just to "keep up with current events". This has been an extremely successful strategy for me when cutting Twitter from my life with my [Twitter Rollup](https://github.com/nozzlegear/twitter-rollup) tool.

## Running the tool

This tool is built for .NET Standard 1.0 (dotnet core) using the new CSPROJ format. To run it, you'll need to install the [dotnet CLI](https://dot.net) on at least version 1.0.0-rc4 (`dotnet --version`).

To run the roundup, just type the following in your terminal (*nix or Windows):

```bash
dotnet build
dotnet /path/to/reddit-rollup.dll
``` 

You can skip sending the email and just output the HTML to a file by passing the `--test` switch:

```bash
dotnet /path/to/reddit-rollup.dll --test
```

To automate the daily email, you'll need to create a cron job on Linux (`crontab -u USERNAME -e`) or use a scheduled task on Windows. **Note that many cron services don't run in a bash shell**! It's probably best to specifically make the cron job execute a bash script that runs your task:

```bash
# Run the Reddit Rollup tool via CRON at 1200 UTC every day, and write its output to ~/reddit-rollup.log
0 12 * * * bash $HOME/reddit-rollup-cron.sh >> $HOME/reddit-rollup.log
```

And in your reddit-rollup-cron.sh:

```bash
#!/bin/bash

# Load environment variables from your .bashenv (or .bashrc)
source ~/.bashenv

# Run reddit rollup
dotnet path/to/reddit-rollup.dll
```


## Environment variables

To simplify email sending, this tool uses [SendWithUs](https://sendwithus.com). You should create an email template that takes the variable `{{rollup_html}}` in the email body, and `{{subject}}` in the subject line.

Add `REDDIT_ROLLUP_SWU_TEMPLATE_ID` with the value of that template's id to your .bashrc or Windows environment variables.

Add `REDDIT_ROLLUP_SWU_KEY` with the value of you SendWithUs API key to your .bashrc or Windows environment variables.