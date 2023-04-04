using Microsoft.AspNetCore.Identity;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SMS_Marketing.API
{
    public class TwilioAPI
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAuthDbContext _authContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public TwilioAPI(ApplicationDbContext context, UserAuthDbContext authDbContext,
                                         UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _authContext = authDbContext;
        }
        public bool PostToTwilio(string? url, string? body, int? id, int? smsGroup)
        {
            try
            {

                if (smsGroup == -1 || smsGroup == null)
                {
                    throw new Exception("No customers were selected");
                }
                Models.Organization? twilioAuth = _context.Organizations
                                            .Where(x => x.Id == id)
                                            .FirstOrDefault();
                var authToken = _context.AppSettings.First(p => p.Index == AppSettingsAccess.TwilioAuthToken).Value;
                var accountSid = _context.AppSettings.First(p => p.Index == AppSettingsAccess.TwilioSID).Value;
                if (authToken == null || accountSid == null || twilioAuth == null) throw new Exception("One of the required tokens is missing, contact the system administrator.");
                List<Customer> customers = new List<Customer>();
                customers = _context.Customers
                            .Where(x => x.GroupId == smsGroup)
                            .ToList();
                TwilioClient.Init(accountSid, authToken);
                if (customers == null) throw new Exception("No customers were selected");
                if (url != null)
                {
                    foreach (var customer in customers)
                    {
                        var mediaUrl = new[] { new Uri(url) }.ToList();
                        var message = MessageResource.Create(
                        body: body,
                        from: new Twilio.Types.PhoneNumber(twilioAuth.TwilioPhoneNumber),
                        mediaUrl: mediaUrl,
                        to: new Twilio.Types.PhoneNumber(customer.PhoneNumber)
                        );
                        Console.WriteLine($"Message to {customer.PhoneNumber} has been {message.Status}.");
                    }
                }
                else
                {
                    foreach (Customer customer in customers)
                    {
                        var message = MessageResource.Create(
                                    body: body,
                                    from: new Twilio.Types.PhoneNumber(twilioAuth.TwilioPhoneNumber),
                                    to: new Twilio.Types.PhoneNumber(customer.PhoneNumber)
                        );
                        Console.WriteLine($"Message to {customer.PhoneNumber} has been {message.Status}.");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Error.InitializeError("Posting Twilio", "007", ex.Message);
                Error.LogError();
                return false;
            }

        }
    }
}
