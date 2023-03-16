using System.Text.RegularExpressions;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace TwilioReceive.Controllers
{
	public class SmsController : TwilioController
	{
		public TwiMLResult Index(SmsRequest incomingMessage)
		{
			string number = incomingMessage.From;
			string sendingnum = incomingMessage.To;
			string subscribeMessage = "Flat";
			string orgName = "Flat Earth News";
			//check to see if number is in database
			bool subscribed = false;
			//check to see if number has name
			bool hasName = false;
			string customersName = "";
			var messagingResponse = new MessagingResponse();
			string privacyURL = "BEEPBOP.com";
			if (subscribed && hasName)
			{
				return TwiML(messagingResponse);
			}
			else
			{
				if (incomingMessage.Body == subscribeMessage)
				{
					messagingResponse.Message($"Thank You {sendingnum} for subscribing to {orgName}. Please Tell us your Name in the format First, Last");
					subscribed = true;//add num to database here
					return TwiML(messagingResponse);
				}
				else
				{
					if (IsFullName(incomingMessage.Body)==true)//&& subscribed
					{
						//we take name and send to database here
						customersName = incomingMessage.Body;
						messagingResponse.Message($"Thank You {customersName} you will now recieve messages from {orgName}");
						subscribed = true;
						return TwiML(messagingResponse);
					}
					else
					{
						
						messagingResponse.Message($"If you are trying to subscribe to {orgName} Please respond with the join message {subscribeMessage} if you are trying to give us your name please send in format First, Last");

						return TwiML(messagingResponse);
					}
				}
			}
		}
		public static bool IsFullName(string str)
		{
			string pattern = @"^[A-Za-z]+,\s[A-Za-z]+$";
			return Regex.IsMatch(str, pattern);
		}
	}
}
