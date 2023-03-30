using LinqToTwitter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;

namespace SMS_Marketing.Controllers
{
    [Authorize]
    public class TwitterController : Controller
    {
        #region Init
        private readonly ApplicationDbContext _context;
        private readonly UserAuthDbContext _authContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public TwitterController(ApplicationDbContext context, UserAuthDbContext authDbContext,
                                         UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _authContext = authDbContext;
        }
        #endregion

        // GET: TwitterController
        public ActionResult Index()
        {
            return View();
        }

        #region User Authentication
        public ActionResult Login(int? id)
        {
            return RedirectToAction("LoginTwitter", new { @id = id });
        }

        //GET: TwitterController/Login/3
        [ActionName("LoginTwitter")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> LoginTwitter(int? id)
        {
            try
            {
                if (id != null)
                {
                    //var twitterAuth = await _context.TwitterAuth.FindAsync(id);
                    var twitterAuth = _context.TwitterAuth
                                   .Where(i => i.OrganizationId == id)
                                   .FirstOrDefault();
                    Organization? organization = await GetCurrentOrg(id);
                    if (organization == null || organization.IsActive == false) throw new Exception("Not a valid organization.");
                    if (twitterAuth == null || organization.IsTwitter == false)
                    {
                        //await auth.CredentialStore.ClearAsync();
                        var auth = new MvcAuthorizer
                        {
                            CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
                            {
                                //ConsumerKey = twitterConfiguration.GetValue<string>("Twitter:Key"),
                                //ConsumerSecret = twitterConfiguration.GetValue<string>("Twitter:Secret")
                                ConsumerKey = _context.AppSettings.First(p => p.Index == AppSettingsAccess.TwitterKey).Value,
                                ConsumerSecret = _context.AppSettings.First(p => p.Index == AppSettingsAccess.TwitterSecret).Value
                            }
                        };
                        //string twitterCallbackUrl = Request.GetDisplayUrl().Replace("Login", "CompleteLogin");
                        TempData["CurrentOrg"] = id;
                        //string twitterCallbackUrl = "https://localhost:7076/Organization/MyOrganizations/";
                        string twitterCallbackUrl = "https://localhost:7076/Twitter/CompleteLogin/";
                        return await auth.BeginAuthorizationAsync(new Uri(twitterCallbackUrl));
                    };
                    if (twitterAuth.AccessToken != null && twitterAuth.OAuthToken != null)
                    {
                        var consumerKey = _context.AppSettings.First(p => p.Index == AppSettingsAccess.TwitterKey).Value;
                        var consumerSecret = _context.AppSettings.First(p => p.Index == AppSettingsAccess.TwitterSecret).Value;
                        var auth = new MvcAuthorizer
                        {
                            CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
                            {
                                OAuthToken = twitterAuth.OAuthToken,
                                OAuthTokenSecret = twitterAuth.AccessToken,
                                ConsumerKey = consumerKey,
                                ConsumerSecret = consumerSecret
                            }
                        };
                        //await SaveTwitterOrgChanges((int)id);
                        return RedirectToAction("Index", "Organization", new { @id = id });
                    }
                }
                else
                {
                    throw new Exception("Invalid Organization Id.");
                }
            }
            catch (Exception ex)
            {
                Error.InitializeError("Twitter Authentication", id.ToString(), ex.Message);
                Error.LogError();
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
            TempData["Error"] += "There has been an unidentified error.";
            return View("Index");
        }

        private async Task<ActionResult> AuthorizeUser()
        {
            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
                {
                    //ConsumerKey = twitterConfiguration.GetValue<string>("Twitter:Key"),
                    //ConsumerSecret = twitterConfiguration.GetValue<string>("Twitter:Secret")
                    ConsumerKey = _context.AppSettings.First(p => p.Index == AppSettingsAccess.TwitterKey).Value,
                    ConsumerSecret = _context.AppSettings.First(p => p.Index == AppSettingsAccess.TwitterSecret).Value
                }
            };
            //await auth.CredentialStore.ClearAsync();
            string twitterCallbackUrl = "https://localhost:7076/Twitter/CompleteLogin/";
            return await auth.BeginAuthorizationAsync(new Uri(twitterCallbackUrl));
        }

        //GET: Twitter/CompleteLogin/
        [ActionName("CompleteLogin")]
        public async Task<ActionResult> CompleteLogin()
        {
            int? id = (int)TempData.Peek("CurrentOrg");
            if (id == null) throw new Exception("Invalid Organization Id");
            try
            {
                //Init Credential Store that holds Twitter Tokens
                var auth = new MvcAuthorizer
                {
                    CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
                };

                //Reads Tokens from URL and stores them
                await auth.CompleteAuthorizeAsync(new Uri(Request.GetDisplayUrl()));
                var credentials = auth.CredentialStore;

                //Creates Auth Object
                TwitterAuth twitterAuth = new();
                //twitterAuth.OrganizationId = (int)organizationId;
                _ = id != null ? twitterAuth.OrganizationId = (int)id : throw new ArgumentException();
                _ = credentials.ScreenName != null ? twitterAuth.UserScreenName = credentials.ScreenName : throw new ArgumentException();
                _ = credentials.OAuthToken != null ? twitterAuth.OAuthToken = credentials.OAuthToken : throw new ArgumentException();
                _ = credentials.OAuthTokenSecret != null ? twitterAuth.AccessToken = credentials.OAuthTokenSecret : throw new ArgumentException();
                _ = credentials.UserID.ToString() != null ? twitterAuth.TwitterId = credentials.UserID.ToString() : throw new ArgumentException();

                await _context.TwitterAuth.AddAsync(twitterAuth);
                await AddTwitterToOrganization((int)id);
                await _context.SaveChangesAsync();
                TempData["Success"] += "Saved Twitter Credentials.";
                return RedirectToAction("Index", "Organization", new { id = id });
            }
            catch (Exception ex)
            {
                Error.InitializeError("Twitter Authentication", id.ToString(), ex.Message);
                Error.LogError();
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
        }

        #endregion

        #region Twitter Service Management

        public async Task<ActionResult> Disable(int? id)
        {
            try
            {
                if (id == null) throw new Exception("Invalid Organization ID");
                Organization? organization = await GetCurrentOrg(id);
                if (organization == null) throw new Exception("Invalid Organization ID");
                organization.IsTwitter = false;
                _context.Organizations.Update(organization);
                //await _context.SaveChangesAsync();
                TempData["Success"] += "Successfully Disabled Twitter.";
                return RedirectToAction("Index", "Organization", new { id = id });
            }
            catch (Exception ex)
            {
                Error.InitializeError("Disable Twitter", id.ToString(), ex.Message);
                Error.LogError();
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
        }

        #endregion

        #region Helper Methods

        private async Task<ActionResult> SaveTwitterOrgChanges(int organizationId)
        {
            try
            {
                Organization? organization = await _context.Organizations.FindAsync(organizationId);
                if (organization != null)
                {
                    _context.Organizations.Update(organization);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Could not update your organization.");
                }
            }
            catch (Exception ex)
            {
                Error.InitializeError("Save Twitter Credentials.", $"Org Id: {organizationId}", ex.Message);
                Error.LogError();
                TempData["Error"] += ex.Message;
                return RedirectToAction("Index", "Error");
            }
            return RedirectToAction("Organization/" + organizationId);
        }

        private async Task<Organization> AddTwitterToOrganization(int id)
        {
            Organization organization = new();
            try
            {
                organization = _context.Organizations
                                .Where(i => i.Id == id)
                                .FirstOrDefault() ?? throw new ArgumentException();
                organization.IsTwitter = true;
                _context.Organizations.Update(organization);
            }
            catch (Exception ex)
            {
                Error.InitializeError("Save Twitter Credentials.", $"Org Id: {id}", ex.Message);
                Error.LogError();
                TempData["Error"] += ex.Message;
            }
            return organization;
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
                Error.InitializeError("Twitter Authentication", id.ToString(), ex.Message);
                Error.LogError();
                TempData["Error"] += ex.Message;
            }
            return null;
        }

        #endregion

        #region For Testing - not in production

        //POST TwitterController/Twitter/CompleteLogin
        [HttpPost]
        [ActionName("Create-Tweet")]
        public async Task<ActionResult> PostTweet(string userPost)
        {
            if (userPost != null)
            {
                try
                {
                    var auth = new MvcAuthorizer
                    {
                        CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
                    };
                    var ctx = new TwitterContext(auth);
                    var parameter = new Dictionary<string, string?>
                    {
                        {"status", userPost }
                    };
                    string queryString = "/statuses/update.json";

                    string result = await ctx.ExecuteRawAsync(queryString, parameter, HttpMethod.Post);
                    if (result != null)
                    {
                        ViewBag.Result = result;
                        return RedirectToAction("ViewTweets");
                    }
                    else
                    {
                        ViewBag.Result = "Result is null";
                    }
                }
                catch (Exception ex)
                {
                    Error.InitializeError("Twitter Posting", "No Org Id", ex.Message);
                    Error.LogError();
                    TempData["Error"] += ex.Message;
                    return RedirectToAction("Index", "Error");
                }

            }
            return RedirectToAction("Index", "Home");
        }

        // GET: TwitterController/ViewTweets
        [ActionName("ViewTweets")]
        public async Task<ActionResult> ViewTweets()
        {
            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
            };

            var ctx = new TwitterContext(auth);

            var tweets =
                await
                (from tweet in ctx.Status
                 where tweet.Type == StatusType.Home
                 select new TweetViewModel
                 {
                     ImageUrl = tweet.User.ProfileImageUrl,
                     ScreenName = tweet.User.ScreenNameResponse,
                     Text = tweet.Text
                 })
                .ToListAsync();
            if (tweets != null)
            {
                ViewBag.Tweets = tweets;
            }
            else
            {
                ViewBag.Tweets = null;
            }
            return View();
        }

        #endregion
    }
}
