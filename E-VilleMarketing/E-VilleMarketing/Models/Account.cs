using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_VilleMarketing.Models
{
    public class Account
    {
        [Key]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
