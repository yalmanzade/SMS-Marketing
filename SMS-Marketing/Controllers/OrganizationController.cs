using LinqToTwitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SMS_Marketing.API;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;
using Twilio.TwiML.Messaging;

namespace SMS_Marketing.Controllers;

[Authorize]
public class OrganizationController : Controller
{
    #region "Properties"
    private readonly ApplicationDbContext _context;
    private readonly UserAuthDbContext _authContext;
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    #endregion

    #region Constructor
    public OrganizationController(ApplicationDbContext context, UserAuthDbContext authDbContext,
                                     UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _authContext = authDbContext;
    }

    #endregion

    // GET: OrganizationController
    public async Task<ActionResult> Index(int? id)
    {
        Organization? organization = new();
        try
        {
            organization = await GetCurrentOrg(id);
            organization.Groups = GetGroups(id);
            organization.CurrentUser = await GetCurrentUser();
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
            return RedirectToAction("Index", "Error");
        }
        return View(organization);
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
                if (organizationList.Count == 1)
                {
                    var organizationId = organizationList.FirstOrDefault().Id;
                    return RedirectToAction("Index", new { id = organizationId });
                }
                else if (organizationList.Count == 0)
                {
                    if (appUser.OrganizationId == -1) throw new Exception("Could not find your organizations.");
                    return RedirectToAction("Index", new { id = appUser.OrganizationId });
                }
                ViewBag.OrganizationList = organizationList;
                return View(organizationList);
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
            RedirectToAction("Index", "Error");
        }
        return View();
    }

    #region Posting to Twilio, Facebook and Twitter

        //POST: Organization/Index
        [HttpPost]
        [ActionName("SubmitPost")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitPost(string postText, IFormFile postPicture, int? id, int smsGroup, string? url,
                                                   IFormCollection collection)
        {
            if (id == null) throw new Exception("This is not a valid Organization ID");
            try
            {
                //Creates new post object for our records.
                Post post = InitPost(collection);
                if (post.IsServices == false)
                {
                    TempData["Error"] += "You did not select a service.";
                    return RedirectToAction("Index", "Organization", new { @id = id });
                }

                //Checks to see if there are any checkboxes checked
                if (post.IsServices == false)
                {
                    TempData["Error"] += "You did not select a service.";
                    return RedirectToAction("Index", "Organization", new { @id = id });
                }

            //Checks if we need to post to Twitter
            if (post.OnTwitter)
            {
                //Twitter : Gets organization's credentials/tokens
                var auth = new MvcAuthorizer
                {
                    CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
                };
                if (auth.CredentialStore.OAuthToken == null)
                {
                    auth.CredentialStore = await GetCredentialStore(id);
                }
                //Twitter : Gets organization's Twitter Context
                TwitterContext twitterContext = new(auth);

                //Init Twitter API
                TwitterAPI twitterAPI = new(postText, postPicture, url, twitterContext);

                //Posting to Twitter
                bool result = true;
                //bool result = await twitterAPI.PostTweet();

                //If Tweet was posted successfully
                if (result)
                {
                    TempData["Success"] += "Tweet was posted successfully.";
                }
                else
                {
                    post.OnTwitter = false;
                    TempData["Error"] += "Failed to post tweet.";
                }
            }

                if (post.OnSMS)
                {
                    //bool result = await PostToTwilio(url, postText, id, smsGroup);
                    TwilioAPI twilioAPI = new TwilioAPI(_context, _authContext, _userManager, _signInManager);
                    bool result = twilioAPI.PostToTwilio(url, postText, id, smsGroup);
                    if (result == true)
                    {
                        TempData["Success"] += "Texts were sent successfully.";
                        post.OnSMS = true;
                    }
                    else
                    {
                        TempData["Error"] += "Failed to send texts.";
                        post.OnSMS = false;
                    }
                }
                if (post.OnFacebook)
                {
                    //bool result = await PostToTwilio(url, postText, id, smsGroup);
                    TwilioAPI twilioAPI = new TwilioAPI(_context, _authContext, _userManager, _signInManager);
                    bool result = twilioAPI.PostToTwilio(url, postText, id, smsGroup);
                    if (result == true)
                    {
                        TempData["Success"] += "Texts were sent successfully.";
                        post.OnSMS = true;
                    }
                    else
                    {
                        TempData["Error"] += "Failed to send texts.";
                        post.OnSMS = false;
                    }
                }
                if (post.OnFacebook)
                {

                    FacebookAPI facebook = new FacebookAPI(_context, _authContext, _userManager, _signInManager);
                    bool result = await facebook.PostToFacebook(postText, postPicture, id);
                    if (result)
                    {
                        TempData["Success"] += " Facebook successfully posted.";
                    }
                    else
                    {
                        post.OnFacebook = false;
                        TempData["Error"] += " Facebook failed to post.";

                    }
                }

            //Checks to see if the post is Valid
            if (post.IsServices)
            {
                Organization? organization = await GetCurrentOrg(id);
                AppUser appUser = await GetCurrentUser();

                //Add more info to the post object
                post.OrganizationId = organization.Id;
                post.OrganizationName = organization.Name;
                post.AuthorId = appUser.Id;
                post.AuthorName = $"{appUser.FirstName} {appUser.LastName}";
                post.Success = true;

                    //Saves post to database
                    await LogPost(post);
                }
                else
                {

                    TempData["Error"] += "Could not post to sevices.";
                }
                return RedirectToAction("Index", new { id = id });
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
        }

    public async Task<SessionStateCredentialStore> GetCredentialStore(int? id)
    {
        var credentialStore = new SessionStateCredentialStore(HttpContext.Session);
        try
        {
            TwitterAuth? twitterAuth = _context.TwitterAuth
                       .Where(x => x.OrganizationId == id)
                       .FirstOrDefault();

            if (twitterAuth != null)
            {

                credentialStore.ConsumerKey = _context.AppSettings.First(p => p.Index == AppSettingsAccess.TwitterKey).Value;
                credentialStore.ConsumerSecret = _context.AppSettings.First(p => p.Index == AppSettingsAccess.TwitterSecret).Value;
                credentialStore.OAuthTokenSecret = twitterAuth.AccessToken;
                credentialStore.OAuthToken = twitterAuth.OAuthToken;
            }
            else
            {
                throw new Exception("We could not find your credentials. Please activate Twitter.");
            }
        }

        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
            RedirectToAction("Index", "Error");
        }
        return credentialStore;
    }

        //This method logs a post to the database.
        private async Task LogPost(Post post)
        {
            try
            {
                if (post == null) throw new Exception("Null Post");
                await _context.Posts.AddAsync(post);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var currentSystem = $"Failed to log post for {post.OrganizationName}.";
                Error.InitializeError(currentSystem, "100", ex.Message);
                Error.LogError();
            }
        }

    private static Post InitPost(IFormCollection collection)
    {
        Post post = new Post();
        try
        {
            if (collection["IsTwitter"] == "on") post.OnTwitter = true; else post.OnTwitter = false;
            if (collection["IsFacebook"] == "on") post.OnFacebook = true; else post.OnFacebook = false;
            if (collection["IsSMS"] == "on") post.OnSMS = true; else post.OnSMS = false;
        }
        catch (Exception ex)
        {
            var currentSystem = $"Failed to init post";
            Error.InitializeError(currentSystem, "100", ex.Message);
            Error.LogError();
        }
        return post;
    }
    #endregion

    #region User Management

    // GET: OrganizationController/UserManagement/5
    public async Task<ActionResult> UserManagement(int? id)
    {
        Organization? organization = new Organization();
        try
        {
            if (id == null) throw new Exception("Invalid Id.");
            organization = await GetCurrentOrg(id);
            organization.AppUsers = await GetCurrentUsers(id);
            organization.CurrentUser = await GetCurrentUser();
            if (organization.AppUsers == null) throw new Exception();
            return View(organization);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            RedirectToAction("Index", "Error");
        }
        return View(organization);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("AddUser")]
    public async Task<ActionResult> AddUser(int? id, string? email)
    {
        try
        {
            if (email == null) throw new Exception("Bad Input");
            var targetUser = _authContext.Users
                .Where(x => x.Email == email)
                .ToList().FirstOrDefault();
            if (targetUser == null) throw new Exception("User does not exist.");
            var currentUser = await GetCurrentUser();
            if (currentUser == null) throw new Exception("User does not exist.");
            var currentOrganization = await GetCurrentOrg(id);
            if (currentOrganization == null) throw new Exception("We could not retrieve your data.");

            //Checks if invite already exists
            var currentInvites = _context.Invites
                        .Where(x => x.InvitingOrganizationId == currentOrganization.Id
                                && x.TargetEmail == email).ToList();
            if (!currentInvites.IsNullOrEmpty())
            {
                TempData["Success"] = $"An invite was already sent to {email}";
                return RedirectToAction("UserManagement", new { id = id });
            }

            //Else

            Invite invite = new();
            invite.AuthorId = currentUser.Id;
            invite.AuthorName = $"{currentUser.FirstName} {currentUser.LastName}";
            invite.TargetUserId = targetUser.Id;
            invite.TargetEmail = email;

            invite.InvitingOrganizationId = currentOrganization.Id;
            await _context.Invites.AddAsync(invite);
            TempData["Success"] += $"Invite was sent to {email}";
            await _context.SaveChangesAsync();
            return RedirectToAction("UserManagement", new { id = id });
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
        }
        //if (ViewData["Error"] != null) return View("Index", "Error");
        TempData["Error"] += "There was an unexpected error.";
        return View("Index", "Error");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("ModifyPermissions")]
    public async Task<ActionResult> ModifyPermissions(string? id, IFormCollection collection)
    {
        try
        {
            int? orgId = int.Parse(collection["orgId"]);
            if (id == null) throw new Exception("Invalid User Id");
            AppUser? targetUser = await _authContext.Users.FindAsync(id);
            if (targetUser == null) throw new Exception("Could not find user.");
            var activeCondition = targetUser.IsActive;
            targetUser.IsActive = collection["user.IsActive"][0] == "true" ? true : false;
            if (!targetUser.IsActive)
            {
                targetUser.ResetPermissions();
                _authContext.Users.Update(targetUser);
                await _authContext.SaveChangesAsync();
                TempData["Success"] = $"{targetUser.FirstName} was disabled.";
                return RedirectToAction("UserManagement", new { id = orgId });
            }
            else if (targetUser.IsActive && activeCondition == false)
            {
                targetUser.IsActive = true;
                _authContext.Users.Update(targetUser);
                await _authContext.SaveChangesAsync();
                TempData["Success"] = $"{targetUser.FirstName} was enabled.";
                return RedirectToAction("UserManagement", new { id = orgId });
            }
            targetUser.IsPost = collection["user.IsPost"][0] == "true" ? true : false;
            targetUser.IsUserManagement = collection["user.IsUserManagement"][0] == "true" ? true : false;
            targetUser.IsInsight = collection["user.IsInsight"][0] == "true" ? true : false;
            targetUser.IsCustomerManagment = collection["user.IsCustomerManagment"][0] == "true" ? true : false;
            _authContext.Users.Update(targetUser);
            await _authContext.SaveChangesAsync();
            TempData["Success"] = $"{targetUser.FirstName} was modified.";
            return RedirectToAction("UserManagement", new { id = orgId });
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
        }
        TempData["Error"] += "There was an unexpected error.";
        return View("Index", "Error");
    }

    [ActionName("RemoveUser")]
    public async Task<ActionResult> RemoveUser(string? id)
    {
        try
        {
            if (id == null) throw new Exception("Invalid Id.");
            AppUser? appUser = await _authContext.Users.FindAsync(id);
            if (appUser == null) throw new Exception("Invalid User.");
            int? orgId = appUser.OrganizationId;
            appUser.OrganizationId = -1;
            appUser.ResetPermissions();
            await _authContext.SaveChangesAsync();
            TempData["Success"] = $"{appUser.FirstName} {appUser.LastName} was removed.";
            return RedirectToAction("UserManagement", new { id = orgId });
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
            return View("Index", "Error");
        }
    }
    #endregion

    #region Customer and Group Management

    // GET: OrganizationController/Create
    public async Task<ActionResult> Customers(int? id)
    {
        try
        {
            if (id != null)
            {
                var organization = await GetCurrentOrg(id);
                organization.CurrentUser = await GetCurrentUser();
                var customers = _context.Customers
                            .Where(x => x.OrganizationId == id)
                            .ToList();
                var group = _context.Groups
                            .Where(x => x.OrganizationId == id)
                            .ToList();

                CustomerViewModel customersViewModel = new CustomerViewModel(organization, group, customers);
                TempData["Success"] = "Customers Retrieved";
                return View(customersViewModel);
            }

        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction("Index", "Error");
    }

    #endregion

    #region Insights

    // GET : OrganizationController/Insights/3
    public async Task<ActionResult> Insights(int? id)
    {
        try
        {
            if (id == null) throw new Exception("Invalid Id.");
            Organization? organization = await _context.Organizations.FindAsync(id);
            organization.CurrentUser = await GetCurrentUser();
            if (organization == null) throw new Exception("Organization was not found.");
            organization.RecentPosts = await FetchPosts(id);
            return View(organization);
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return View("Index", "Error");
        }
    }

    private async Task<List<Post>> FetchPosts(int? id)
    {
        List<Post> posts = new();
        try
        {
            if (id == null) throw new Exception("Null Organization Id");
            posts = _context.Posts
                             .Where(x => x.OrganizationId == id)
                             .Take(5)
                             .ToList();
            return posts;
        }
        catch (Exception ex)
        {
            var currentSystem = $"Failed to log fetch posts";
            Error.InitializeError(currentSystem, "100", ex.Message);
            Error.LogError();
        }
        return null;
    }

    #endregion

    #region Helper Methods

    private List<Group>? GetGroups(int? id)
    {
        try
        {
            List<Group>? groups = new();
            if (id != null)
            {
                groups = _context.Groups
                         .Where(g => g.OrganizationId == id)
                         .ToList();
                //if (groups != null) ViewData["CurrentOrg"] = groups;
                if (groups != null) return groups;
            }
            if (groups == null)
            {
                throw new ArgumentNullException("We could not retieve your organization's groups.");
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
            RedirectToAction("Index", "Error");
        }
        return null;
    }

    private async Task<List<AppUser>> GetCurrentUsers(int? id)
    {
        List<AppUser> appUsers = new();
        try
        {
            appUsers = _authContext.Users
                        .Where(u => u.OrganizationId == id)
                        .ToList();
            if (appUsers == null) throw new Exception("We could not retrieve the user list for your organization.");
            return appUsers;
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
        }
        return null;
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

    private async Task<AppUser> GetCurrentUser()
    {
        AppUser appUser = new();
        try
        {
            if (_signInManager.IsSignedIn(User))
            {
                AppUser? user = await _userManager.GetUserAsync(User);
                return user;
            }
        }
        catch (Exception ex)
        {
            TempData["Error"] += ex.Message;
        }
        return null;
    }

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

    #endregion

}
