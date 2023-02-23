using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_VilleMarketing.Models
{
    public class Business
    {
        [Key]
        public int BusinessID { get; set; }
        public string BusinessName { get; set; }
        public int BuildingNum { get; set; }
        public int? AptNum { get; set; }
        public string StreetName { get; set; }
        public int ZipCode { get; set; }
        public string State { get; set; }

        [ForeignKey("Client")]
        public int ClientID { get; set; }
        public Client? Client { get; set; }


        public ICollection<User>? Users { get; set; }


        public ICollection<Facebook>? FacebookAccounts { get; set; }


        public ICollection<TikTok>? TikTokAccounts { get; set; }


        public ICollection<Twitter>? TwitterAccounts { get; set; }


        public ICollection<Twilio>? TwilioNumbers { get; set; }


        public ICollection<Customer>? Customers { get; set; }
    }
}
