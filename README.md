# Reddit Rollup

In an effort to break my addiction to Reddit, I've created this small command line utility. The goal is to go out and get the top 3 posts from a list of subreddits every morning and email them to myself. Together with making an effort to *only* use Reddit via this daily email, I'm aiming to cut the habit of wasting half my day browsing mindlessly and telling myself that it's just to "keep up with current events". This has been an extremely successful strategy for me when cutting Twitter from my life with my [Twitter Rollup](https://github.com/nozzlegear/twitter-rollup) tool.

## Running the tool

This tool is built with F# and .NET Standard. The easiest way to run it is using Docker:

```bash
docker pull nozzlegear/reddit-rollup
docker run --env-file path/to/.env --rm nozzlegear/reddit-rollup
```

Or, build the image yourself using the Dockerfile found in this repo:

```bash
docker build -t username/reddit-rollup
docker run --env-file path/to/.env --rm username/reddit-rollup
```

To automate the daily email, you'll need to create a cron job on Linux or use a scheduled task on Windows. Set up a cron job on Linux by creating a file named `reddit-rollup` in `/etc/cron.d` and place the following content inside: 

```bash
# Run the Reddit Rollup tool via CRON at 1200 UTC every day, and write its output to /var/log/reddit-rollup.log
# NOTE that many cron services don't run in a bash shell, and cron jobs in cron.d must also specify the user.
0 12 * * * username docker run --env-file path/to/.env --rm nozzlegear/reddit-rollup >> /var/log/reddit-rollup.log
```

## Environment variables

To simplify email sending, this tool uses [SendWithUs](https://sendwithus.com). You should create an email template that takes the variable `{{rollup_html}}` in the email body, and `{{subject}}` in the subject line.

Variable|Description
--------|-----------
`REDDIT_ROLLUP_SWU_KEY`|Your sendwithus.com key.
`REDDIT_ROLLUP_SWU_TEMPLATE_ID`|Your sendwithus.com template id.
`REDDIT_ROLLUP_SUB_LIST`|A comma-separated list of subreddits to fill the email with.
`REDDIT_ROLLUP_SENDER`|Email address of the sender, e.g. reddit-rollup@example.com
`REDDIT_ROLLUP_RECIPIENT`|Email address of the recipient, e.g. example@example.com