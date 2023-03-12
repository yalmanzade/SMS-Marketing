using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        public int GroupId { get; set; } = 0;
        [DataType(DataType.Text)]
        [StringLength(20)]
        public string? GroupName { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        [StringLength(20)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [StringLength(12)]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
