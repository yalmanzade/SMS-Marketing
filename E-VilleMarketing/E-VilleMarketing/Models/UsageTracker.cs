using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VilleMarketing.Models
{
    public class UsageTracker
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Business")]
        public int BusinessID { get; set; }
        public Business Business { get; set; }
        public int TwilioMsgSent { get; set; }
        public bool TwitterSent { get; set; }
        public bool FacebookSent { get; set; }
        public bool TikTokSent { get; set; }
    }
}
