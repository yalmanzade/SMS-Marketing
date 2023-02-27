namespace SMS_Marketing.Models
{
    public class TweetViewModel
    {
        public string ImageUrl { get; set; }
        public string ScreenName { get; set; }
        public string Text { get; set; }
    }
    public class MediaViewModel
    {
        public string MediaUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
    }
}
