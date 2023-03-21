using System.ComponentModel.DataAnnotations;

namespace SMS_Marketing.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;
        [DataType(DataType.Text)]
        [StringLength(50)]
        public string Description { get; set; } = string.Empty;
        [Required]
        public bool IsDefault { get; set; } = false;
    }
}
