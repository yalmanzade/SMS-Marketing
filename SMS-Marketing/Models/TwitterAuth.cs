using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class TwitterAuth
    {
        [Key]
        public string OrganizationId { get; set; }
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string OAuthToken { get; set; }
        public string AccessToken { get; set; }
    }
}
