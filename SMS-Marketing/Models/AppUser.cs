using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
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
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;
        [DisplayName("Organization Id")]
        public int OrganizationId { get; set; } = -1;
        [DisplayName("Active")]
        public bool IsActive { get; set; } = false;
        [DisplayName("Posting")]
        public bool IsPost { get; set; } = false;
        [DisplayName("User Management")]
        public bool IsUserManagement { get; set; } = false;
        [DisplayName("Insights")]
        public bool IsInsight { get; set; } = false;
        [DisplayName("Customer Management")]
        public bool IsCustomerManagment { get; set; } = false;
        [DisplayName("System Manager")]
        public bool IsSystemManager { get; set; } = false;
        public void ResetPermissions()
        {
            IsActive = false;
            IsPost = false;
            IsUserManagement = false;
            IsInsight = false;
            IsCustomerManagment = false;
            IsSystemManager = false;
        }
        public void SetOrgManagerPermissions()
        {
            IsActive = true;
            IsPost = true;
            IsUserManagement = true;
            IsInsight = true;
            IsCustomerManagment = true;
            IsSystemManager = false;
        }
    }
}
