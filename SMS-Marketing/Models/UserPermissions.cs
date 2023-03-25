using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    //This model is not being used. It is just for testing.
    public class UserPermissions
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
