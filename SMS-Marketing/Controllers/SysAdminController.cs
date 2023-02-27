using LinqToTwitter;
using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;

namespace SMS_Marketing.Controllers
{
    public class SysAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAuthDbContext _authContext;

        public SysAdminController(ApplicationDbContext context, UserAuthDbContext authDbContext)
        {
            _context = context;
            _authContext = authDbContext;
        }

        // GET: SysAdminController
        public ActionResult Index()
        {
            ViewBag.OrganizationList = _context.Organizations.ToList();
            return View();
        }


        // GET: SysAdminController/Delete/5
        public async Task<ActionResult> Disable(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                var organization = _context.Organizations.Find(id);
                if (organization != null)
                {
                    organization.IsActive = false;
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                    ViewBag.Success = "The organization was disabled.";
                }

                if (organization == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error += ex.Message;
            }
            return RedirectToAction("Index");
        }
        // GET: SysAdminController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            Organization? organization = new();
            try
            {
                if (id == null || id == 0)
                {
                    return NotFound();
                }
                organization = await _context.Organizations.FindAsync(id);

                if (organization == null)
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error += ex.Message;
            }
            return View(organization);
        }
        //POST
        [HttpPost, ActionName("Disable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisablePOST(int? id)
        {
            try
            {
                var obj = _context.Organizations.Find(id);
                if (obj == null)
                {
                    return NotFound();
                }
                obj.IsActive = false;
                _context.Organizations.Update(obj);
                await _context.SaveChangesAsync();
                ViewBag.Success = "Organization Diabled deleted successfully";
            }
            catch (Exception ex)
            {
                ViewBag.Error += ex.Message;
            }
            return RedirectToAction("Index");
        }

        // GET: SysAdminController/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.UserList = _authContext.Users.ToList();
            }
            catch (Exception ex)
            {
                ViewBag.Error += ex.Message;
            }
            return View();
        }

        // POST: SysAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<ActionResult> Create(IFormCollection collection, Category obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string? OrganizationName = collection["Name"];
                    string? ManagerId = collection["ManagerId"];
                    if (OrganizationName == null || ManagerId == null) ViewBag.Error = "We could not create the organization. Please try again.";
                    Organization organization = new();
                    organization.IsActive = true;
                    organization.Name = OrganizationName;
                    organization.ManagerId = ManagerId;
                    var OrgUser = _authContext.Users.Find(ManagerId);
                    if (OrgUser == null) ViewBag.Error = "We could not create the organization. Please try again.";
                    organization.ManagerName = $"{OrgUser.FirstName} {OrgUser.LastName}";
                    var PostedOganization = _context.Organizations.Add(organization);
                    await _context.SaveChangesAsync();
                }
                throw new Exception("There was a problem with the form. Please try again later.");
            }
            catch (Exception ex)
            {
                ViewBag.Error += ex.Message;
            }
            return View("Index");
        }

        // GET: SysAdminController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var organization = _context.Organizations.Find(id);

            if (organization == null)
            {
                return NotFound();
            }
            ViewBag.UserList = _authContext.Users.ToList();
            return View(organization);
        }

        // POST: SysAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IFormCollection collection, int? id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Organization? organization = new();
                    if (id != null)
                    {
                        organization = await _context.Organizations.FindAsync(id);
                    }
                    else
                    {
                        ViewBag.Error = "We could not find this organization.";
                        return RedirectToAction(nameof(Index));
                    }
                    string OrganizationName = collection["Name"];
                    string ManagerId = collection["ManagerId"];
                    if (OrganizationName == null || ManagerId == null) throw new Exception("We could not create the organization. Please try again.");
                    if (collection["IsActive"] == "True") organization.IsActive = true;
                    if (collection["IsActive"] != "True") organization.IsActive = false;
                    organization.Name = OrganizationName;
                    organization.ManagerId = ManagerId;
                    var OrgUser = _authContext.Users.Find(ManagerId);
                    if (OrgUser == null) throw new Exception("We could not create the organization. Please try again.");
                    organization.ManagerName = $"{OrgUser.FirstName} {OrgUser.LastName}";
                    //Update Twilio Information
                    string twilioNumber = collection["TwilioPhoneNumber"];
                    if (twilioNumber == null || twilioNumber.Length < 10)
                    {
                        organization.IsSMS = false;
                        _context.Update(organization);
                        await _context.SaveChangesAsync();
                        throw new ArgumentNullException("Invalid Phone Number Entered. Twilio Disabled");
                    }
                    organization.TwilioPhoneNumber = twilioNumber;
                    organization.IsSMS = true;
                    _context.Organizations.Update(organization);
                    await _context.SaveChangesAsync();
                    ViewBag.Success = "Organization updated successfully.";
                    return RedirectToAction("Index");
                }
                throw new Exception("There was an unknown issue. Please try again.");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage += ex.Message;
                return View();
            }
        }
        //GET : SysAdminController/Settings/Id
        public async Task<ActionResult> Settings(string? id)
        {
            try
            {
                if (id == null) id = "TWITTER";
                if (id != null)
                {
                    ViewBag.SettingsKey = id.ToUpper();
                    List<AppSettings> appSettings = new List<AppSettings>();
                    appSettings = (List<AppSettings>)(from e in _context.AppSettings
                                                      where e.SettingGroup == id.ToUpper()
                                                      select e).ToList();
                    if (appSettings == null || appSettings.Count < 1) throw new Exception("Failed to initialize App Settings.");
                    View(appSettings);
                }
                else
                {
                    throw new Exception("Invalid Route");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage += ex.Message;
            }
            return View();
        }
        //POST: SysAdmin/PostSettings
        public async Task<ActionResult> PostSettings(string? setting, int? index)
        {
            try
            {
                if (setting != null && index != null)
                {
                    var currentSetting = (from e in _context.AppSettings
                                          where e.Index == index
                                          select e).FirstOrDefault();
                    if (currentSetting != null)
                    {
                        currentSetting.Value = setting;
                        _context.Update(currentSetting);
                        await _context.SaveChangesAsync();
                        ViewBag.Success = "Settings Updated";
                    }
                    else
                    {
                        throw new Exception("We failed to update the setting. Please try again.");
                    }
                }
                else
                {
                    throw new Exception("Invalid form. Please try again.");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage += ex.Message;
            }
            return RedirectToAction("Settings");
        }
    }
}
