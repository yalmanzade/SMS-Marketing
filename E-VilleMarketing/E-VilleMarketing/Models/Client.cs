using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VilleMarketing.Models
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        public string Client_FName { get; set; }
        public string Client_LName { get; set; }
        public string Client_Password { get; set; }
        public string Client_Email { get; set; }
        public ICollection<Business>? Businesses {get; set;}
    }
}
