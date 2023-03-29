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

namespace SMS_Marketing.Controllers
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

            Organization organization = _context.Organizations.FirstOrDefault(org => org.TwilioPhoneNumber.Replace(" ", "") == sendingnum);
            int? OrgsId = organization?.Id;
            string? orgName = organization?.Name;

            Customer? customer = _context.Customers.FirstOrDefault(c => c.PhoneNumber.Replace(" ", "") == number && c.OrganizationId == OrgsId);
            var messagingResponse = new MessagingResponse();
            if (customer != null)
            {
                if (IsFullName(incomingMessage.Body) == true)//&& subscribed
                {
                    //we take name and send to database here
                    customersName = incomingMessage.Body;
                    messagingResponse.Message($"Thank You {customersName} you will now recieve messages from {orgName}");
					string[] nameParts = customersName.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
					string firstName = nameParts[0].Trim().Substring(0, Math.Min(nameParts[0].Trim().Length, 30));
					string lastName = nameParts.Length > 1 ? nameParts[1].Trim().Substring(0, Math.Min(nameParts[1].Trim().Length, 20)) : "";

					//subscribed = true;//Needs top be removed
					//need to write logic to take name and add to existing coustomers
					customer.FirstName = firstName;
                    customer.LastName = lastName;
                    if (TryValidateModel(customer))
                    {
                        _context.Customers.Update(customer);

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

                    messagingResponse.Message($"If you are trying to subscribe to {orgName} Please respond with the join message {subscribeMessage} if you are trying to give us your name please send in format First Last");

                    return TwiML(messagingResponse);
                }


                //else
                messagingResponse.Message($"{sendingnum} you are already subscribed to {orgName}.");
                return TwiML(messagingResponse);
            }
            else
            {

                if (incomingMessage.Body.ToLower().Replace(" ", "") == subscribeMessage)
                {
                    messagingResponse.Message($"Thank You {sendingnum} for subscribing to {orgName}. Please Tell us your Name in the format First, Last");
                    Customer tempcustomer = new();
                    int organizationId = OrgsId.GetValueOrDefault();
                    tempcustomer.OrganizationId = organizationId;
                    tempcustomer.PhoneNumber = number;
                    //add to group
                    Models.Group? group = _context.Groups
                                   .Where(g => g.OrganizationId == OrgsId && g.IsDefault == true)
                                   .FirstOrDefault();
                    if (group == null) throw new Exception("Invalid Data. Please try again.");
                    tempcustomer.GroupId = group.Id;
                    tempcustomer.GroupName = group.Name;
                    tempcustomer.FirstName = "TempName";
                    tempcustomer.LastName = "TempLast";



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

                    messagingResponse.Message($"If you are trying to subscribe to {orgName} Please respond with the join message {subscribeMessage} if you are trying to give us your name please send in format First, Last Bottom");//remove bottom later

                    return TwiML(messagingResponse);

                }


            }
        }


        public static bool IsFullName(string str)
        {
            string pattern = @"^[A-Za-z]+([, ]+[A-Za-z]+)?$";
            return Regex.IsMatch(str, pattern);
        }
    }
}
