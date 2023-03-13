using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class TwilioAuth
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string AccountSid { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Text)]
        public string AuthToken { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Text)]
        public string SendingNumber { get; set; } = string.Empty;
    }
}
