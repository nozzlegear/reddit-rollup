using Flurl;
using System;
using System.Threading.Tasks;

namespace reddit_rollup
{
    class Reddit
    {
        private string SECRET { get; set; }

        private string CLIENT_ID { get; set;}

        private const string BASE_PATH = "https://reddit.com/api";

        public Reddit(string accessToken)
        {
            SECRET = Environment.GetEnvironmentVariable("REDDIT_ROLLUP_SECRET");
            CLIENT_ID = Environment.GetEnvironmentVariable("REDDIT_ROLLUP_CLIENT_ID");
        }

        public async Task GetAccessToken()
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
        }
    }
}