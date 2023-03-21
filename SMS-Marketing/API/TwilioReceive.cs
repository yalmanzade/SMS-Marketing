using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;
using System;
using System.Text.RegularExpressions;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using Twilio.Types;

namespace SMS_Marketing.API
{
	public class TwilioReceive : TwilioController
	{
		#region "Properties"
		private readonly ApplicationDbContext _context;
		private readonly UserAuthDbContext _authContext;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		#endregion
		#region Constructor
		public TwilioReceive(ApplicationDbContext context, UserAuthDbContext authDbContext,
										 UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
			_authContext = authDbContext;
		}

		#endregion
		public TwiMLResult Index(SmsRequest incomingMessage)
		{
			string number = incomingMessage.From;
			string sendingnum = incomingMessage.To;
			string subscribeMessage = "subscribe";
			
			//check to see if number is in database
			bool subscribed = false;//needs to be removed
			//check to see if number has name
			bool hasName = false;//needs to be removed
			string customersName = "";

			Organization organization = _context.Organizations.FirstOrDefault(org => org.TwilioPhoneNumber == sendingnum);
			int? OrgsId = organization?.Id;
			string? orgName = organization?.Name;

			Customer customer = _context.Customers.FirstOrDefault(c => c.PhoneNumber == number && c.OrganizationId == OrgsId);
			var messagingResponse = new MessagingResponse();
			if (customer != null)
			{
				messagingResponse.Message($"{sendingnum} you are already subscribed to {orgName}.");
				return TwiML(messagingResponse);
			}
			else
			{
				if (subscribed && hasName)//needs to be removed
				{
					return TwiML(messagingResponse);//needs to be removed
				}
				else
				{
					if (incomingMessage.Body.ToLower() == subscribeMessage)
					{
						messagingResponse.Message($"Thank You {sendingnum} for subscribing to {orgName}. Please Tell us your Name in the format First, Last");
						Customer tempcustomer = new();
						int organizationId = OrgsId.GetValueOrDefault();
						tempcustomer.OrganizationId = organizationId;
						tempcustomer.PhoneNumber = number;
						if (TryValidateModel(tempcustomer))
						{
							_context.Customers.Add(tempcustomer);
							_context.SaveChanges();
						}
						else
						{
							messagingResponse.Message($"Sorry {sendingnum} We are having a problem subscribing you.");
						}
						return TwiML(messagingResponse);
					}
					else
					{
						if (IsFullName(incomingMessage.Body) == true)//&& subscribed
						{
							//we take name and send to database here
							customersName = incomingMessage.Body;
							messagingResponse.Message($"Thank You {customersName} you will now recieve messages from {orgName}");
							subscribed = true;
							//need to write logic to take name and add to existing coustomers
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
		}


		public static bool IsFullName(string str)
		{
			string pattern = @"^[A-Za-z]+,\s[A-Za-z]+$";
			return Regex.IsMatch(str, pattern);
		}
	}
}
