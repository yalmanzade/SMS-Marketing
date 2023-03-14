using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class Authorization
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        [Required]
        public bool IsActive { get; set; } = false;
        [Required]
        public bool IsPost { get; set; } = false;
        [Required]
        public bool IsUserManagement { get; set; } = false;
        [Required]
        public bool IsInsight { get; set; } = false;
        [Required]
        public bool IsCustomerManagment { get; set; } = false;
    }
}
