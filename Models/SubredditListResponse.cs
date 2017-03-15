using System.Collections.Generic;

namespace reddit_rollup.Models
{
    class SubredditListResponse
    {
        public string kind { get; set; }

        public SubredditListData data { get; set; }
    }

    public class SubredditListData
    {
        public string modhash { get; set; }

        public List<Post> children { get; set; }

        public object after { get; set; }

        public object before { get; set; }
    }

    public class Post
    {
        public string kind { get; set; }
        public PostData data { get; set; }
    }

    public class PostData
    {
        public bool? contest_mode { get; set; }

        public object banned_by { get; set; }

        public object media_embed { get; set; }

        public string subreddit { get; set; }

        public string selftext_html { get; set; }

        public string selftext { get; set; }

        public bool? likes { get; set; }

        public object suggested_sort { get; set; }

        public List<object> user_reports { get; set; }

        public object secure_media { get; set; }

        public string link_flair_text { get; set; }

        public string id { get; set; }

        public int? gilded { get; set; }

        public object secure_media_embed { get; set; }

        public bool? clicked { get; set; }

        public int? score { get; set; }

        public object report_reasons { get; set; }

        public string author { get; set; }

        public bool? saved { get; set; }

        public List<object> mod_reports { get; set; }

        public string name { get; set; }

        public string subreddit_name_prefixed { get; set; }

        public object approved_by { get; set; }

        public bool? over_18 { get; set; }

        public string domain { get; set; }

        public bool? hidden { get; set; }

        public string thumbnail { get; set; }

        public string subreddit_id { get; set; }

        public dynamic edited { get; set; }

        public string link_flair_css_class { get; set; }

        public object author_flair_css_class { get; set; }

        public int? downs { get; set; }

        public bool? brand_safe { get; set; }

        public bool? archived { get; set; }

        public object removal_reason { get; set; }

        public bool? is_self { get; set; }

        public bool? hide_score { get; set; }

        public bool? spoiler { get; set; }

        public string permalink { get; set; }

        public object num_reports { get; set; }

        public bool? locked { get; set; }

        public bool? stickied { get; set; }

        public double? created { get; set; }

        public string url { get; set; }

        public object author_flair_text { get; set; }

        public bool? quarantine { get; set; }

        public string title { get; set; }

        public double? created_utc { get; set; }

        public object distinguished { get; set; }

        public object media { get; set; }

        public int? num_comments { get; set; }

        public bool? visited { get; set; }

        public string subreddit_type { get; set; }

        public int? ups { get; set; }

        public Preview preview { get; set; }
    }

    public class Preview 
    {
        public IEnumerable<Image> images { get; set; }

        public bool enabled { get; set; }
    }

    public class Image : Media
    {
        public Variant variants { get; set; }
        
        public string id { get; set; }
    }

    public class Media 
    {
        public MediaSource source { get; set; }
        
        public List<MediaSource> resolutions { get; set; }
    }

    public class MediaSource
    {
        public string url { get; set; }
        
        public int width { get; set; }
        
        public int height { get; set; }
    }

    public class Variant
    {
        public Media obfuscated { get; set; }
        
        public Media nsfw { get; set; }
        
        public Media gif { get; set; }
        
        public Media mp4 { get; set; }
    }

    public class MediaEmbed
    {
    }

    public class SecureMediaEmbed
    {
    }
}