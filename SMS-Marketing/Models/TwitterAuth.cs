using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class TwitterAuth
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Organization ID")]
        public int OrganizationId { get; set; }
        //[Required]
        //[DataType(DataType.Text)]
        //public string ConsumerKey { get; set; } = string.Empty;
        //[Required]
        //[DataType(DataType.Text)]
        //public string ConsumerSecret { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Text)]
        public string UserScreenName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Text)]
        public string OAuthToken { get; set; } = string.Empty;
        //AKA Token Secret
        [Required]
        [DataType(DataType.Text)]
        public string AccessToken { get; set; } = string.Empty;
        [Required]
        [DisplayName("Twitter Id")]
        public string TwitterId { get; set; } = string.Empty;
    }
}
