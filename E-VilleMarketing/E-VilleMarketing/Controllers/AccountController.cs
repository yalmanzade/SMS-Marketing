using Microsoft.AspNetCore.Mvc;
using E_VilleMarketing.Models;
using Microsoft.EntityFrameworkCore;
using E_VilleMarketing.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Immutable;

namespace E_VilleMarketing.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;
        public string passedLayout = "null";
        public AccountController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult LoginView()
        {
            return View();
        }
        public IActionResult RegisterClient()
        {
            return View();
        }
        public IActionResult Login()
        {
            string emailInput = HttpContext.Request.Form["email"];
            string password = HttpContext.Request.Form["password"];
            foreach (var Login in _context.Logins.ToList())
            {
                if (Login.Email == emailInput && Login.Password == password) 
                {
                    var clientContext = _context.Clients.Where(b => b.Client_Email == emailInput).IsNullOrEmpty();
                    var userContext = _context.Users.Where(b => b.User_Email == emailInput).IsNullOrEmpty();
                    if (clientContext != true)
                    {
                        HttpContext.Session.SetInt32("clientID", _context.Clients.Single(e => e.Client_Email == emailInput).ClientID);
                        return RedirectToAction("Index", "Businesses");
                    }
                    if (userContext != true)
                    {
                        HttpContext.Session.SetInt32("userID", _context.Users.Single(e => e.User_Email == emailInput).UserID);
                        HttpContext.Session.SetInt32("businessID", _context.Users.Single(e => e.User_Email == emailInput).BusinessID);
                        return RedirectToAction("Index", "Composition");
                    }
                }
            }
            return RedirectToAction("Error", "Account");
        }
        public IActionResult Register()
        {
            var emailInput = HttpContext.Request.Form["email"];
            foreach (var Login in _context.Logins.ToList())
            {
                if (Login.Email == emailInput)
                {
                    return View("Error");
                }
            }
            var firstName = HttpContext.Request.Form["FirstName"];
            var lastName = HttpContext.Request.Form["LastName"];
            var password = HttpContext.Request.Form["password"];
            Client newClient = new Client();
            newClient.Client_FName = firstName;
            newClient.Client_LName = lastName;
            newClient.Client_Email = emailInput;
            newClient.Client_Password = password;
            Account account = new Account();
            account.Email = emailInput;
            account.Password = password;
            _context.Clients.Add(newClient);
            _context.Logins.Add(account);
            _context.SaveChanges();
            var clientQuery = _context.Clients.First(e => e == newClient);
            HttpContext.Session.SetInt32("ClientID", clientQuery.ClientID);
            return RedirectToAction("Index", "Business");
        }
    }
}
