using System.Collections.Generic;
using Newtonsoft.Json;

namespace reddit_rollup.Models
{
    public class SendWithUsData
    {
        [JsonProperty("email_id")]
        public string EmailId { get; set; }

        [JsonProperty("recipient")]
        public Email Recipient { get; set; }

        [JsonProperty("sender")]
        public Email Sender { get; set; }

        [JsonProperty("email_data")]
        public object EmailData { get; set; }

        /// <summary>
        /// Adds the given files to the email as attachments.
        /// </summary>
        [JsonProperty("files")]
        public List<SendWithUsFile> Files { get; set; } = new List<SendWithUsFile>();

        [JsonProperty("cc")]
        public List<Email> CC { get; set; } = new List<Email>();

        [JsonProperty("bcc")]
        public List<Email> BCC { get; set; } = new List<Email>();
    }

    public class Email
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("reply_to")]
        public string ReplyTo { get; set; }
    }

    public class RollupEmailData
    {
        public RollupEmailData(string html, string subject)
        {
            rollup_html = html;
            this.subject = subject;
        }

        public string rollup_html { get; set; }

        public string subject { get; set; }
    }

    public class SendWithUsFile
    {
        /// <summary>
        /// Creates a new <see cref="SendWithUsFile"/> to add as an attachment to a SendWithUs email.
        /// </summary>
        /// <param name="id">The file's filename.</param>
        /// <param name="data">The file's base64-encoded data</param>
        public SendWithUsFile(string id, string data)
        {
            Id = id;
            Data = data;
        }

        /// <summary>
        /// The file's filename.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The file's base64-encoded data.
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }
    }

    public class SendWithUsResponse
    {
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }

        public string Status { get; set; }

        [JsonProperty("receipt_id")]
        public string ReceiptId { get; set; }

        public Email Email { get; set; }
    }
}