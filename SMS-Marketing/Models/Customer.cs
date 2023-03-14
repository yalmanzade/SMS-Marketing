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
        [Required]
        [DataType(DataType.Text)]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(30)]
        public string LastName { get; set; }
        [Required]
        [StringLength(12)]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
