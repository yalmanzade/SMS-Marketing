using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VilleMarketing.Models
{
    public class Twilio
    {
        [Key]
        public int TwilioID { get; set; }
        public string TWilioNum { get; set; }
        [ForeignKey("Business")]
        public int BusinessID { get; set; }
        public Business Business { get; set; }
    }
}
