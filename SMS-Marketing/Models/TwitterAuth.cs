using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class TwitterAuth
    {
        [Key]
        [Required]
        [DisplayName("Organization ID")]
        public int OrganizationId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string ConsumerKey { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string ConsumerSecret { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string OAuthToken { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string AccessToken { get; set; }
        [Required]
        [DisplayName("Twitter Id")]
        public string TwitterId { get; set; }
    }
}
