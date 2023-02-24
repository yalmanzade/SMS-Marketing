using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;

namespace _2twilio.Controllers
{
    public class SendController : Controller
    {
        public IActionResult Send()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Send(string accountSid, string authToken, string fromNumber, string toNumber1, string url, string body)
        {
            TwilioClient.Init(accountSid, authToken);
            if(url != null) 
            {
                var mediaUrl = new[] { new Uri(url) }.ToList();
                var message = MessageResource.Create(
                body: body,
                from: new Twilio.Types.PhoneNumber(fromNumber),
                mediaUrl: mediaUrl,
                to: new Twilio.Types.PhoneNumber(toNumber1)
                );
                Console.WriteLine(message.Sid);
            }
            else
            {
                var message = MessageResource.Create(
                body: body,
                from: new Twilio.Types.PhoneNumber(fromNumber),
                to: new Twilio.Types.PhoneNumber(toNumber1)
                );
                Console.WriteLine(message.Sid);
            }
            return RedirectToAction("Send");
        }
    }
}
