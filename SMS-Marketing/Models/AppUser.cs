using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string LastName { get; set; } = string.Empty;
        public string OrganizationId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public bool IsPost { get; set; } = false;
        public bool IsUserManagement { get; set; } = false;
        public bool IsInsight { get; set; } = false;
        public bool IsCustomerManagment { get; set; } = false;
        public bool IsSystemManager { get; set; } = false;
    }
}
