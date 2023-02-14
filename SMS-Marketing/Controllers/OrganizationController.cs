using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;

namespace SMS_Marketing.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAuthDbContext _authContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public OrganizationController(ApplicationDbContext context, UserAuthDbContext authDbContext,
                                         UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _authContext = authDbContext;
        }

        // GET: OrganizationController
        public async Task<ActionResult> Index(int? id)
        {
            Organization? organization = new();
            try
            {
                if (id != null)
                {
                    organization = await _context.Organizations.FindAsync(id);
                    ViewBag.Success = "Organization Retrieved";
                    if (organization != null) return View(organization);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }

        // GET: OrganizationController
        [ActionName("MyOrganizations")]
        public async Task<ActionResult> MyOrganizations()
        {
            try
            {
                AppUser? appUser = new();
                if (_signInManager.IsSignedIn(User))
                {
                    appUser = await _userManager.GetUserAsync(User);
                }
                if (appUser != null)
                {

                    List<Organization> organizationList = _context.Organizations.Where(organization => organization.ManagerId == appUser.Id).ToList();
                    ViewBag.OrganizationList = organizationList;
                    return View(organizationList);
                }
            }
            catch (Exception ex)
            {


            }
            return View();
        }

        // GET: OrganizationController/UserManagement/5
        public async Task<ActionResult> UserManagement(int id)
        {
            return View();
        }

        // GET: OrganizationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrganizationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: OrganizationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrganizationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: OrganizationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrganizationController/Delete/5
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
    }
}
