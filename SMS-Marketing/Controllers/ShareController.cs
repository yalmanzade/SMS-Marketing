using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;
using System.Text.RegularExpressions;

namespace SMS_Marketing.Controllers
{
    public class ShareController : Controller
    {
        #region Properties

        private readonly ApplicationDbContext _context;
        private readonly UserAuthDbContext _authContext;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        #endregion
        #region Constructor

        public ShareController(UserAuthDbContext userAuth, ApplicationDbContext context, ILogger<HomeController> logger, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManage)
        {
            _userManager = userManager;
            _signInManager = signInManage;
            _logger = logger;
            _context = context;
            _authContext = userAuth;
        }

        #endregion

        #region Subscribe and Unsubscribe

        // GET: ShareController
        public async Task<ActionResult> Index(int? id)
        {
            try
            {
                if (id == null) throw new Exception("This organization is not valid.");
                Organization? organization = await _context.Organizations.FindAsync(id);
                if (organization == null) throw new Exception("Organization not found.");
                FacebookAuth? facebook = _context.FacebookAuth.Where(e => e.OrganizationId == id).FirstOrDefault();
                TwitterAuth? twitter = _context.TwitterAuth.Where(e => e.OrganizationId == id).FirstOrDefault();
                Customer customer = new()
                {
                    OrganizationId = organization.Id,
                    Organization = organization,
                    Facebook = facebook,
                    Twitter = twitter
                };
                return View(customer);
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: ShareController/Subscribe
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Subscribe")]
        public async Task<ActionResult> Subscribe(Customer customerForm)
        {
            try
            {
                if (customerForm == null) throw new Exception("Invalid Data. Please try again.");
                if (ModelState.IsValid)
                {
                    var prefix = customerForm.PhoneNumber[..1];
                    if (prefix != "+1")
                    {
                        customerForm.PhoneNumber = "+1" + customerForm.PhoneNumber;
                    }
                    if (PhoneIsValid(customerForm.PhoneNumber) == false) throw new Exception("Invalid Phone Number.");

                    //Checks if user already exists.
                    Customer? customer = await _context.Customers.FirstOrDefaultAsync(
                        x => x.PhoneNumber == customerForm.PhoneNumber && x.OrganizationId == customerForm.Id);
                    if (customer != null) return View("SubscribeSuccess");

                    Models.Group? group = _context.Groups
                                   .Where(g => g.OrganizationId == customerForm.Id && g.IsDefault == true)
                                   .FirstOrDefault();
                    if (group == null) throw new Exception("Invalid Data. Please try again.");
                    Customer newCustomer = new()
                    {
                        OrganizationId = customerForm.Id,
                        FirstName = customerForm.FirstName,
                        LastName = customerForm.LastName,
                        GroupId = group.Id,
                        GroupName = group.Name,
                        PhoneNumber = customerForm.PhoneNumber,
                    };
                    await _context.AddAsync(newCustomer);
                    await _context.SaveChangesAsync();
                    return View("SubscribeSuccess");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
            TempData["Error"] += "There has been an error.";
            return RedirectToAction("Index", "Error");
        }

        // GET: ShareController/Unsubscribe
        public async Task<ActionResult> Unsubscribe(int? id)
        {
            try
            {
                if (id == null) throw new Exception("Invalid Organization. Please try again");
                return View(id);
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: ShareController/UnsubscribeUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UnsubscribeUser")]
        public async Task<ActionResult> UnsubscribeUser(string? phoneN)
        {
            try
            {
                if (phoneN == null) throw new Exception("Invalid Phone Number.");
                var prefix = phoneN.Substring(0, 1);
                if (prefix != "+1")
                {
                    phoneN = "+1" + phoneN;
                }
                if (PhoneIsValid(phoneN) == false) throw new Exception("Invalid Phone Number.");
                var rows = 0;
                rows = await _context.Customers
                             .Where(x => x.PhoneNumber == phoneN)
                             .ExecuteDeleteAsync();
                return View("UnsubscribeSuccess");
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
        }

        // GET: ShareController/UnsubscribeSuccess
        public ActionResult UnsubscribeSuccess()
        {
            return View();
        }

        // GET: ShareController/SubscribeSuccess
        public ActionResult SubscribeSuccess()
        {
            return View();
        }
        #endregion

        // POST: ShareController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #region Helper Methods
        public bool PhoneIsValid(string phone)
        {
            //const string bluePrint = "^([\\+]?1[-]?|[0])?[1-9][0-9]{8}$";
            const string bluePrint = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            return true;
            return Regex.IsMatch(phone, bluePrint);
        }
        #endregion
    }
}
