using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VilleMarketing.Models
{
    public class Facebook
    {
        [Key]
        public int FacebookID { get; set; }
        public string FacebookAppId { get; set; } 
        public string Facebook_Acc_Token { get; set; }
        [ForeignKey("Business")]
        public int BusinessID { get; set; }
        public Business Business { get; set; }
    }
}
