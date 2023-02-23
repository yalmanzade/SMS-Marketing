using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VilleMarketing.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public string Customer_FName { get; set; } 
        public string Customer_LName { get; set; } 
        public int PhoneNum { get; set; }
        [ForeignKey("Business")]
        public int BusinessID { get; set; }
        public ICollection<Business>? Businesses { get; set; }

    }
}
