using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;

namespace SMS_Marketing.Controllers
{
    public class FacebookController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAuthDbContext _authContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public FacebookController(ApplicationDbContext context, UserAuthDbContext authDbContext,
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
        #region Add Facebook Functionality
        public async Task<IActionResult> AddFacebook(int? id)
        {
            try
            {
                if (id != null)
                {
                    var organization = await GetCurrentOrg(id);
                    TempData["Success"] = "Organization Retrieved";
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
                string AppId = HttpContext.Request.Form["AppId"];
                string AccessToken = HttpContext.Request.Form["AccessToken"];
                string UserScreenName = HttpContext.Request.Form["UserScreenName"];
                FacebookAuth facebookAuth = new FacebookAuth();
                facebookAuth.OrganizationId = id.GetValueOrDefault();
                facebookAuth.AppId = AppId;
                facebookAuth.AccessToken = AccessToken;
                facebookAuth.UserScreenName = UserScreenName;
                if (TryValidateModel(facebookAuth))
                {
                    _context.FacebookAuth.Add(facebookAuth);
                    _context.SaveChanges();
                }
                if (!TryValidateModel(facebookAuth))
                    return RedirectToAction("Index", "Error");
            return RedirectToAction("Index", "Organization", new { @id = id });
        }
        #endregion
    }
}
