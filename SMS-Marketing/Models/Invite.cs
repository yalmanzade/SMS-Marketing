using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class Invite
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int InvitingOrganizationId { get; set; }
        [Required]
        public string TargetUserId { get; set; } = string.Empty;
        [Required]
        public string AuthorName { get; set; } = string.Empty;
        [Required]
        public string AuthorId { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string TargetEmail { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime SentDate { get; set; } = DateTime.Now;
        [DataType(DataType.DateTime)]
        public DateTime AcceptedDate { get; set; }
        [Required]
        public bool IsAccepted { get; set; } = false;
    }
}
