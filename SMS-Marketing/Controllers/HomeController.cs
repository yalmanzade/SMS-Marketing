using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;
using System.Diagnostics;

namespace SMS_Marketing.Controllers;

[Authorize]
public class HomeController : Controller
{
    #region Properties

    private readonly ApplicationDbContext _context;
    private readonly UserAuthDbContext _authContext;
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    #endregion
    #region Constructor

    public HomeController(UserAuthDbContext userAuth, ApplicationDbContext context, ILogger<HomeController> logger, UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManage)
    {
        _userManager = userManager;
        _signInManager = signInManage;
        _logger = logger;
        _context = context;
        _authContext = userAuth;
    }

    #endregion
    public async Task<IActionResult> Index()
    {
        if (_signInManager.IsSignedIn(User))
        {
            AppUser? user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                ViewBag.Invites = await GetInvites(user.Id);
            }
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public ActionResult AboutUs()
    {
        return View();
    }

    #region Invite

    [HttpPost]
    [ActionName("DeclineInvite")]
    public async Task<ActionResult> DeclineInvite(int? id)
    {
        try
        {
            if (id == null) throw new Exception("Invalid Parameters.");
            var invite = await GetSingleInvite(id);
            if (invite == null) throw new Exception("Error deleting invite.");
            _context.Invites.Remove(invite);
            await _context.SaveChangesAsync();
            TempData["Success"] += "Invitation Decliened.";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
        }
        return RedirectToAction("Index", "Error");
    }
    [HttpPost]
    [ActionName("AcceptInvite")]
    public async Task<ActionResult> AcceptInvite(int? id)
    {
        try
        {
            if (id == null) throw new Exception("Invalid Parameters.");
            if (_signInManager.IsSignedIn(User))
            {
                AppUser? user = await _userManager.GetUserAsync(User);
                if (user == null) throw new Exception("Please log in again.");

                //Organization owners can't be part of other organization due to the limitations of the AppUser Model
                //We will need to create a better authorization system system.
                if (user.IsOrgManager) throw new Exception($"{user.FullName}, Organization Owners Can't Be Part of Other Oganizations.");
                if (user.OrganizationId != -1) throw new Exception($"{user.FullName}, You Can't Be Part of Another Oganizations.");
                Invite invite = new();
                invite = await GetSingleInvite(id);
                invite.AcceptedDate = DateTime.UtcNow;
                invite.IsAccepted = true;
                user.OrganizationId = invite.InvitingOrganizationId;

                //This set all permissions to false.
                user.ResetPermissions();
                _authContext.Users.Update(user);
                _context.Invites.Update(invite);
                await _context.SaveChangesAsync();
                await _authContext.SaveChangesAsync();
                TempData["Success"] += "Invitation Accepted.";
                return View("Index");
            }
        }
        catch (Exception ex)
        {
            ViewData["Error"] += ex.Message;
        }
        return RedirectToAction("Index", "Error");
    }

    #endregion


    #region Helper Methods

    private async Task<List<Invite>> GetInvites(string? id)
    {
        try
        {
            if (id == null) throw new Exception("We could not find invites.");
            List<Invite> invites = new();
            invites = _context.Invites
                      .Where(x => x.TargetUserId == id && x.IsAccepted == false)
                      .ToList();
            if (invites != null && invites.Count > 0) return invites;
            return null;
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
        }
        return null;
    }
    private async Task<Invite> GetSingleInvite(int? id)
    {
        try
        {
            Invite? invite = new();
            invite = await _context.Invites.FindAsync(id);
            if (invite == null) return null;
            return invite;
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
        }
        return null;
    }

    #endregion

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}