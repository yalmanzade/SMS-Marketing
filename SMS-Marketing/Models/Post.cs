using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class Post
    {
        [Key]
        [Required]
        [DisplayName("Id")]
        public int Id { get; set; }

        [Required]
        [DisplayName("Organization ID")]
        public int? OrganizationId { get; set; } = -1;

        [Required]
        [DisplayName("Organization")]
        public string OrganizationName { get; set; } = string.Empty;

        [DisplayName("Post Date")]
        public DateTime PostedDate { get; set; } = DateTime.Now;

        [Required]
        [DisplayName("Author Id")]
        [DataType(DataType.Text)]
        public string AuthorId { get; set; } = string.Empty;

        [Required]
        [DisplayName("Posted By")]
        [DataType(DataType.Text)]
        public string AuthorName { get; set; } = string.Empty;

        [Required]
        [DisplayName("On Twitter")]
        public bool OnTwitter { get; set; } = false;

        [Required]
        [DisplayName("On SMS")]
        public bool OnSMS { get; set; } = false;

        [Required]
        [DisplayName("On Facebook")]
        public bool OnFacebook { get; set; } = false;

        [Required]
        [DisplayName("Success")]
        public bool Success { get; set; } = false;
    }
}
