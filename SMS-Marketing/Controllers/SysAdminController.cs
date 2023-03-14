﻿using LinqToTwitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;

namespace SMS_Marketing.Controllers
{
    [Authorize]
    public class SysAdminController : Controller
    {
        #region Init

        private readonly ApplicationDbContext _context;
        private readonly UserAuthDbContext _authContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public SysAdminController(ApplicationDbContext context, UserAuthDbContext authDbContext,
                                  UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _authContext = authDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #endregion

        // GET: SysAdminController
        public async Task<ActionResult> Index()
        {
            try
            {
                //Authentication Starts
                //AppUser user = await GetCurrentUser();
                //if (IsSysManager(user) == false) throw new Exception("You do not have access to this page.");
                // End Authentication
                ViewBag.OrganizationList = _context.Organizations.ToList();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Error");
            }
        }

        #region Organization Management

        //This region handles creating, disabling and other operations relating to 
        //managing organizations.

        // GET: SysAdminController/Create 
        public async Task<ActionResult> Create()
        {
            try
            {
                //Authentication Starts
                //AppUser user = await GetCurrentUser();
                //if (IsSysManager(user) == false) throw new Exception("You do not have access to this page.");
                // End Authentication
                ViewBag.UserList = _authContext.Users.ToList();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Error");
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
                //Authentication Starts
                //AppUser user = await GetCurrentUser();
                //if (IsSysManager(user) == false) throw new Exception("You do not have access to this page.");
                // End Authentication

                if (ModelState.IsValid)
                {
                    //Gathers and checks data
                    string? organizationName = collection["Name"];
                    string? managerId = collection["ManagerId"];
                    if (organizationName == null || managerId == null) throw new Exception("Invalid Parameters.");
                    AppUser? organizationManager = await _authContext.Users.FindAsync(managerId.ToString());
                    if (organizationManager == null) throw new Exception("We could not find this user.");

                    //Creates new Organization
                    Organization organization = new();
                    organization.IsActive = true;
                    organization.Name = organizationName;
                    organization.ManagerId = organizationManager.Id;
                    organization.ManagerName = $"{organizationManager.FirstName} {organizationManager.LastName}";
                    await _context.Organizations.AddAsync(organization);

                    //Set permissions for Organization Manager
                    organizationManager.SetOrgManagerPermissions();
                    _authContext.Users.Update(organizationManager);

                    //Creates default group for the organization
                    Group group = new Group()
                    {
                        OrganizationId = organization.Id,
                        Name = "All Users",
                        Description = "This group contains all users",
                        IsDefault = true
                    };
                    await _context.Groups.AddAsync(group);

                    //Updates both contexts
                    await _authContext.SaveChangesAsync();
                    await _context.SaveChangesAsync();
                    return View("Index");
                }
                throw new Exception("There was a problem with the form. Please try again later.");
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
        }

        public async Task<ActionResult> Disable(int? id)
        {
            try
            {
                //Authentication Starts
                //AppUser user = await GetCurrentUser();
                //if (IsSysManager(user) == false) throw new Exception("You do not have access to this page.");
                // End Authentication

                if (id == null) throw new Exception("Invalid Id.");
                var organization = await _context.Organizations.FindAsync(id);
                if (organization == null) throw new Exception("Invalid organization. Try again.");
                organization.IsActive = false;
                List<AppUser> appUsers = new();
                appUsers = _authContext.Users.Where(x => x.OrganizationId == id).ToList();
                if (appUsers != null && appUsers.Count > 0)
                {
                    appUsers.ForEach(user =>
                    {
                        user.IsActive = false;
                        user.ResetPermissions();
                        user.OrganizationId = -1;
                        _authContext.Users.Update(user);
                    });
                }
                int rows = await _context.Invites
                                 .Where(x => x.InvitingOrganizationId == id)
                                 .ExecuteDeleteAsync();
                _context.Organizations.Update(organization);
                await _context.SaveChangesAsync();
                await _authContext.SaveChangesAsync();
                TempData["Success"] += "The organization was disabled.";
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
            return RedirectToAction("Index");
        }

        // GET: SysAdminController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            Organization? organization = new();
            try
            {
                //Authentication Starts
                //AppUser user = await GetCurrentUser();
                //if (IsSysManager(user) == false) throw new Exception("You do not have access to this page.");
                // End Authentication

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
                TempData["Error"] += ex.Message;
            }
            return View(organization);
        }

        // GET: SysAdminController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                //Authentication Starts
                //AppUser user = await GetCurrentUser();
                //if (IsSysManager(user) == false) throw new Exception("You do not have access to this page.");
                // End Authentication

                if (id == null) throw new ArgumentNullException("Id is not valid.");
                var organization = await _context.Organizations.FindAsync(id);
                if (organization == null) throw new Exception("Invalid Organization");
                ViewBag.UserList = _authContext.Users.ToList();
                return View(organization);
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
        }

        // POST: SysAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(IFormCollection collection, int? id)
        {
            try
            {
                //Authentication Starts
                //AppUser user = await GetCurrentUser();
                //if (IsSysManager(user) == false) throw new Exception("You do not have access to this page.");
                // End Authentication

                if (ModelState.IsValid)
                {
                    Organization? organization = new();
                    if (id != null)
                    {
                        organization = await _context.Organizations.FindAsync(id);
                    }
                    else
                    {
                        TempData["Error"] = "We could not find this organization.";
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
                    TempData["Success"] = "Organization updated successfully.";
                    return RedirectToAction("Index");
                }
                throw new Exception("There was an unknown issue. Please try again.");
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                return View();
            }
        }

        #endregion

        #region Insights

        public ActionResult Insights()
        {
            return View();
        }

        #endregion

        #region App Settings

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
                TempData["Error"] += ex.Message;
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
                        _context.AppSettings.Update(currentSetting);
                        await _context.SaveChangesAsync();
                        TempData["Success"] += "Settings Updated";
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
                TempData["Error"] += ex.Message;
            }
            return RedirectToAction("Settings");
        }

        #endregion

        #region Helper Methods

        // Checks if user is the System Manager.
        private static bool IsSysManager(AppUser appUser)
        {
            try
            {
                if (appUser == null) throw new Exception("Please log in to perform this operation.");
                if (appUser.IsSystemManager == false) throw new Exception("You do not have access to perform this action.");
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Gets current User
        private async Task<AppUser> GetCurrentUser()
        {
            AppUser appUser = new();
            try
            {
                if (_signInManager.IsSignedIn(User))
                {
                    AppUser? user = await _userManager.GetUserAsync(User);
                    if (user != null) return user;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
            }
            return null;
        }
        #endregion
    }
}