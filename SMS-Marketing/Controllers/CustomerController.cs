using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;

namespace SMS_Marketing.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAuthDbContext _authContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public CustomerController(ApplicationDbContext context, UserAuthDbContext authDbContext,
                                         UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _authContext = authDbContext;
        }
        private async Task<Organization?> GetCurrentOrg(int? id)
        {
            try
            {
                Organization? organization = new();
                if (id != null)
                {
                    organization = await _context.Organizations.FindAsync(id);
                    if (organization != null) ViewData["CurrentOrg"] = organization;
                    TempData["Success"] += "Organization Retrieved";
                    if (organization != null) return organization;
                }
                if (organization == null)
                {
                    throw new ArgumentNullException("We could not retieve your organization.");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                RedirectToAction("Index", "Error");
            }
            return null;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateCustomer(int? id)
        {
            try
            {
                if (id != null)
                {
                    var organization = await GetCurrentOrg(id);
                    TempData["Success"] = "Customers Retrieved";
                    return View(organization);
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(int? id)
        {
            string FName = HttpContext.Request.Form["FirstNameInput"];
            string LName = HttpContext.Request.Form["LastNameInput"];
            string PNum = HttpContext.Request.Form["PhoneNumberInput"];
            int? Id = id;
            Customer customer = new();
            customer.OrganizationId = Id.GetValueOrDefault();
            customer.FirstName = FName;
            customer.LastName = LName;
            customer.PhoneNumber = PNum;
            Group TempGroup = _context.Groups.Where(e => e.OrganizationId == id && e.IsDefault == true).FirstOrDefault();
            customer.GroupId = TempGroup.Id;
            customer.GroupName = TempGroup.Name;
            var prefix = customer.PhoneNumber[..2];
            if (prefix != "+1")
            {
                customer.PhoneNumber = "+1" + customer.PhoneNumber;
            }
            if (TryValidateModel(customer))
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
            }
            if (!TryValidateModel(customer))
                return RedirectToAction("Index", "Error");
            return RedirectToAction("Customers", "Organization", new { @id = id });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            var customer = _context.Customers.Where(e => e.Id == id).FirstOrDefault();
            customer.IsActive = false;
            _context.SaveChanges();
            return RedirectToAction("Customers", "Organization", new { @id = customer.OrganizationId });
        }
    }
}
