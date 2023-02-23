using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VilleMarketing.Models
{
    [Index(nameof(User_Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string User_Email { get; set; }
        public string User_Password { get; set; }
        [ForeignKey("Business")]
        public int BusinessID { get; set; }
        public Business? Business { get; set; }
    }
}
