using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class Costumer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [DataType(DataType.Text)]
        [StringLength(20)]
        public int GroupName { get; set; }
        [DataType(DataType.Text)]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        [StringLength(20)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [StringLength(12)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;
        [StringLength(30)]
        [DataType(DataType.Text)]
        public string Description { get; set; } = string.Empty;
    }
}
