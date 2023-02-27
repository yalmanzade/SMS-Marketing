using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class FacebookAuth
    {
        public int Id { get; set; }
        [Required]
        [DisplayName("Organization ID")]
        public int OrganizationId { get; set; }
        [Required]
        [DisplayName("Facebook App ID")]
        public string AppId { get; set; } = string.Empty;
        [Required]
        [DisplayName("Facebook Access Token")]
        public string AccessToken { get; set; } = string.Empty;
        [Required]
        [DisplayName("Organization Screen Name")]
        public string UserScreenName { get; set; } = string.Empty;
    }
}
