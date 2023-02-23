using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VilleMarketing.Models
{
    public class Twitter
    {
        [Key]
        public int TwitterID { get; set; }
        public string Twit_Token_1 { get; set; } 
        public string Twit_Token_2 { get; set; } 
        public string Twit_Token_3 { get; set; } 
        public string Twit_Token_4 { get; set; } 
        public DateTime DateCreated { get; set; }
        [ForeignKey("Business")]
        public int BusinessID { get; set; }
        public Business Business { get; set; }
    }
}
