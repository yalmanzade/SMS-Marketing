using LinqToTwitter;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SMS_Marketing.Controllers
{
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
        public async Task<ActionResult> SubmitPost(string postText, IFormFile postPicture, int? id, int smsGroup, string? url,
                                                   IFormCollection collection)
        {
            try
            {
                //var auth = new MvcAuthorizer
                //{
                //    CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
                //};
                //if (auth.CredentialStore.OAuthToken == null)
                //{
                //    auth.CredentialStore = await GetCredentialStore(id);
                //}
                //TwitterContext twitterContext = new(auth);
                //if (twitterContext != null && postPicture != null && postText != null)
                //{
                //    bool facebookSuccess = await PostToFacebookImage(postText, postPicture, id);
                //    string mediaCategory = "tweet_image";
                //    var fileStream = postPicture.OpenReadStream();
                //    byte[] bytes = new byte[fileStream.Length];
                //    using (fileStream)
                //    {
                //        fileStream.Read(bytes, 0, (int)postPicture.Length);
                //    }
                //    var imageUploads = new List<Task<Media>>
                //    {
                //        twitterContext.UploadMediaAsync(bytes, postPicture.ContentType,mediaCategory),
                //    };
                //    await Task.WhenAll(imageUploads);
                //    List<string> mediaIds =
                //        (from tsk in imageUploads
                //         select tsk.Result.MediaID.ToString())
                //         .ToList();
                //    Tweet? tweet = await twitterContext.TweetMediaAsync(postText, mediaIds);
                //    if (tweet != null)
                //    {
                //        ViewBag.Result += "Message Posted. " + tweet.ID;
                //        return RedirectToAction("Index", "Organization", new { @id = id });
                //    }
                //    else
                //    {
                //        throw new Exception("We could not post your message to Twitter.");
                //    }
                //}
                //if (twitterContext != null && postText != null && postText.Length > 0 && postText.Length < 280)
                //{
                //    var parameter = new Dictionary<string, string?>
                //    {
                //        { "status", postText}
                //    };
                //    string? queryString = "/statuses/update.json";
                //    string? result = await twitterContext.ExecuteRawAsync(queryString, parameter, HttpMethod.Post);
                //    if (result != null)
                //    {
                //        ViewBag.Result += "Message Posted. " + result;
                //        return RedirectToAction("Index", "Organization", new { @id = id });
                //    }
                //    else
                //    {
                //        throw new Exception("Result is null. There has been an error sending your message.");
                //    }
                //}
                //if (true)
                //{
                //    _ = await PostToTwilio(url, postText, id, smsGroup);
                //}
                Organization? organization = await GetCurrentOrg(id);
                AppUser appUser = await GetCurrentUser();
                Post post = InitPost(collection);
                post.OrganizationId = organization.Id;
                post.OrganizationName = organization.Name;
                post.AuthorId = appUser.Id;
                post.AuthorName = $"{appUser.FirstName} {appUser.LastName}";
                post.Success = true;
                await LogPost(post);
            }
            catch (Exception ex)
            {
                TempData["Error"] += ex.Message;
                RedirectToAction("Index", "Error");
            }
            if (id != null)
            {
                return RedirectToAction("Index", new { id = id });
            }
            TempData["Error"] += "Unexpected Error.";
            return RedirectToAction("Index", "Organization", new { @id = id });
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

        private async Task<bool> PostToFacebookImage(string message, IFormFile imageFile, int? id)
        {
            try
            {
                FacebookAuth? facebookAuth = _context.FacebookAuth
                                               .Where(x => x.OrganizationId == id)
                                               .FirstOrDefault();

                if (facebookAuth.AccessToken == null || facebookAuth.AppId == null) return false;
                string accessToken = facebookAuth.AccessToken;
                string pageId = facebookAuth.AppId;
                //string accessToken = _context.AppSettings.First(p => p.Index == AppSettingsAccess.FacebookAccessToken).Value;
                //string pageId = _context.AppSettings.First(p => p.Index == AppSettingsAccess.FacebookAppId).Value;
                var url = $"https://graph.facebook.com/{pageId}/feed?access_token={accessToken}";

                if (imageFile == null)
                {
                    var data = $"message={message}";
                    var dataBytes = Encoding.UTF8.GetBytes(data);

                    using (var client = new HttpClient())
                    {
                        var response = await client.PostAsync(url, new ByteArrayContent(dataBytes));

                        if (response.IsSuccessStatusCode)
                        {
                            TempData["Success"] += "Post was successful.";
                            return true;
                        }
                        else
                        {
                            TempData["Success"] += "Post was unsuccessful.";
                            return false;
                        }
                    }
                }
                else
                {
                    url = $"https://graph.facebook.com/{pageId}/photos?access_token={accessToken}";

                    using (var client = new HttpClient())
                    {
                        using (var content = new MultipartFormDataContent())
                        {
                            content.Add(new StringContent(message), "message");

                            using (var stream = new MemoryStream())
                            {
                                await imageFile.CopyToAsync(stream);
                                content.Add(new ByteArrayContent(stream.ToArray()), "source", imageFile.FileName);
                            }

                            var response = await client.PostAsync(url, content);

                            if (response.IsSuccessStatusCode)
                            {
                                TempData["Success"] += "Post was successful.";
                                return true;
                            }
                            else
                            {
                                TempData["Success"] += "Post was unsuccessful.";
                                return false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Error.InitializeError("Fa", "100", ex.Message);
                Error.LogError();
                return false;
            }
            return false;
        }

        private async Task<bool> PostToTwilio(string? url, string? body, int? id, int? smsGroup)
        {
            try
            {
                Models.TwilioAuth? twilioAuth = _context.TwilioAuth
                                        .Where(x => x.Id == id)
                                        .FirstOrDefault();
                var authToken = "";
                var accountSid = "";
                if (authToken == null) return false;
                if (accountSid == null) return false;
                if (twilioAuth == null) return false;
                List<Customer> customers = new List<Customer>();
                customers = _context.Customers
                            .Where(x => x.GroupId == id)
                            .ToList();
                TwilioClient.Init(accountSid, authToken);
                if (customers == null) return false;
                if (url != null)
                {
                    foreach (var customer in customers)
                    {
                        var mediaUrl = new[] { new Uri(url) }.ToList();
                        var message = MessageResource.Create(
                        body: body,
                        from: new Twilio.Types.PhoneNumber(twilioAuth.SendingNumber),
                        mediaUrl: mediaUrl,
                        to: new Twilio.Types.PhoneNumber(customer.PhoneNumber)
                        );
                        Console.WriteLine($"Message to {customer.PhoneNumber} has been {message.Status}.");
                    }
                }
                foreach (Customer customer in customers)
                {
                    var message = MessageResource.Create(
                                body: body,
                                from: new Twilio.Types.PhoneNumber(twilioAuth.SendingNumber),
                                to: new Twilio.Types.PhoneNumber(customer.PhoneNumber)
                    );
                    Console.WriteLine($"Message to {customer.PhoneNumber} has been {message.Status}.");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                RedirectToAction("Index", "Error");
            }
            return false;
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

        private Post InitPost(IFormCollection collection)
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
                if (targetUser == null) throw new Exception("User does not exist.");
                Invite invite = new();
                invite.AuthorId = currentUser.Id;
                invite.AuthorName = $"{currentUser.FirstName} {currentUser.LastName}";
                invite.TargetUserId = targetUser.Id;
                invite.TargetEmail = email;
                var currentOrganization = await GetCurrentOrg(id);
                if (currentOrganization == null) throw new Exception("We could not retrieve your data.");
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
                targetUser.IsActive = collection["user.IsActive"][0] == "true" ? true : false;
                if (!targetUser.IsActive)
                {
                    targetUser.ResetPermissions();
                    _authContext.Users.Update(targetUser);
                    await _authContext.SaveChangesAsync();
                    TempData["Success"] = $"{targetUser.FirstName} was disabeled.";
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
                int orgId = appUser.OrganizationId;
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
                    var users = _context.Customers
                                .Where(x => x.OrganizationId == id)
                                .ToList();
                    var group = _context.Groups
                                .Where(x => x.OrganizationId == id)
                                .ToList();
                    CustomerViewModel customersViewModel = new CustomerViewModel(organization, group, users);
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
}
