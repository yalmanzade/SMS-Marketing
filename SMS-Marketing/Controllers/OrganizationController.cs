using LinqToTwitter;
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
                organization = await GetCurrentOrg(id);
            }
            catch (Exception ex)
            {
                ViewBag.Error += ex.Message;
                return RedirectToAction("Index", "Error");
            }
            return View(organization);
        }

        private async Task<Organization?> GetCurrentOrg(int? id)
        {
            try
            {
                Organization? organization = new();
                if (id != null)
                {
                    organization = await _context.Organizations.FindAsync(id);
                    ViewBag.Success += "Organization Retrieved";
                    if (organization != null) return organization;
                }
                if (organization == null)
                {
                    throw new ArgumentNullException("We could not retieve your organization.");
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error += ex.Message;
                RedirectToAction("Index", "Error");
            }
            return null;
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
                ViewBag.Error += ex.Message;
                RedirectToAction("Index", "Error");
            }
            return View();
        }
        //POST: Organization/Index
        [HttpPost]
        [ActionName("SubmitPost")]
        public async Task<ActionResult> SubmitPost(string postText, IFormFile postPicture, int? id)
        {
            try
            {
                var auth = new MvcAuthorizer
                {
                    CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
                };
                if (auth.CredentialStore.OAuthToken == null)
                {
                    auth.CredentialStore = await GetCredentialStore(id);
                }
                TwitterContext twitterContext = new(auth);
                if (twitterContext != null && postPicture != null)
                {
                    string mediaCategory = "tweet_image";
                    var fileStream = postPicture.OpenReadStream();
                    byte[] bytes = new byte[fileStream.Length];
                    using (fileStream)
                    {
                        fileStream.Read(bytes, 0, (int)postPicture.Length);
                    }
                    var imageUploads = new List<Task<Media>>
                    {
                        twitterContext.UploadMediaAsync(bytes, postPicture.ContentType,mediaCategory),
                    };
                    await Task.WhenAll(imageUploads);
                    List<string> mediaIds =
                        (from tsk in imageUploads
                         select tsk.Result.MediaID.ToString())
                         .ToList();
                    Tweet? tweet = await twitterContext.TweetMediaAsync(postText, mediaIds);
                    if (tweet != null)
                    {
                        ViewBag.Result += "Message Posted. " + tweet.ID;
                        return RedirectToAction("Index", "Organization", new { @id = id });
                    }
                    else
                    {
                        throw new Exception("We could not post your message to Twitter.");
                    }
                    //MediaViewModel tweetViewModel = new()
                    //{
                    //    MediaUrl = tweet.Entities.Urls.FirstOrDefault()?.Url,
                    //    Description = tweet.Entities.Urls.FirstOrDefault()?.Description,
                    //    Text = tweet.Text
                    //};
                    //return View(tweetViewModel);
                }
                if (twitterContext != null && postText != null && postText.Length > 0 && postText.Length < 280)
                {
                    var parameter = new Dictionary<string, string?>
                    {
                        { "status", postText}
                    };
                    string? queryString = "/statuses/update.json";
                    string? result = await twitterContext.ExecuteRawAsync(queryString, parameter, HttpMethod.Post);
                    if (result != null)
                    {
                        ViewBag.Result += "Message Posted. " + result;
                        return RedirectToAction("Index", "Organization", new { @id = id });
                    }
                    else
                    {
                        throw new Exception("Result is null. There has been an error sending your message.");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error += ex.Message;
                RedirectToAction("Index", "Error");
            }
            if (id != null)
            {
                var organization = await _context.Organizations.FindAsync(id);
                ViewBag.Success = "Organization Retrieved";
                if (organization != null) return View(organization);
            }
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
                ViewBag.Error += ex.Message;
                RedirectToAction("Index", "Error");
            }
            return credentialStore;
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
