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

        public int GroupId { get; set; } = 0;
        [DataType(DataType.Text)]
        [StringLength(20)]

        public string? GroupName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Text)]
        [StringLength(30)]
        public string Name { get; set; }
        [Required]

        [DisplayName("First Name")]
        [DataType(DataType.Text)]
        [StringLength(20)]
        public string Name { get; set; } = string.Empty;

        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        [StringLength(30)]
        public string LastName { get; set; }
        [Required]
        [DisplayName("Phone Number")]
        [StringLength(12)]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;


        [NotMapped]
        public Organization Organization { get; set; } = new();
    }
}
