using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VilleMarketing.Models
{
    public class TikTok
    {
        [Key]
        public int TikTokID { get; set; }
        public string TikTokOpenID { get; set; } 
        public string TikTokAccessToken { get; set; }
        [ForeignKey("Business")]
        public int BusinessID { get; set; }
        public Business Business { get; set; }
    }
}
