using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMS_Marketing.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        [Required]
        public int GroupId { get; set; }

        [DataType(DataType.Text)]
        [StringLength(20)]
        public string GroupName { get; set; } = string.Empty;

        [DisplayName("First Name")]
        [DataType(DataType.Text)]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        [StringLength(20)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DisplayName("Phone Number")]
        [StringLength(12)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [DisplayName("Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [StringLength(30)]
        [DataType(DataType.Text)]
        public string Description { get; set; } = string.Empty;

        [NotMapped]
        public Organization Organization { get; set; } = new();
    }
}
