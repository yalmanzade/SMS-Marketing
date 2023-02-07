using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS_Marketing.Models
{
    public class Post
    {
        [Key]
        [Required]
        [DisplayName("Organization ID")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Body")]
        [DataType(DataType.Text)]
        public string Body { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayName("Post Date")]
        public DateTime EntryDate { get; set; }
        [Required]
        [DisplayName("Posted By")]
        [DataType(DataType.Text)]
        public string Author { get; set; }
        [Required]
        [DisplayName("On Tweeter")]
        public bool OnTweeter { get; set; }
        [Required]
        [DisplayName("On SMS")]
        public bool OnSMS { get; set; }
        [Required]
        [DisplayName("On Facebook")]
        public bool OnFacebook { get; set; }
        [NotMapped]
        public string TweeterBody
        {
            get
            {
                if (Body.Length < 281) return Body;
                return Body.Substring(0, 280);
            }
        }
    }
}
