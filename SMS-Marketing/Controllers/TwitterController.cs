using LinqToTwitter;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using SMS_Marketing.Models;

namespace SMS_Marketing.Controllers
{
    public class TwitterController : Controller
    {
        // GET: TwitterController
        public ActionResult Index()
        {
            return View();
        }
        //GET: TwitterController/Details/5
        public async Task<ActionResult> Login()
        {
            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
                {
                    ConsumerKey = "",
                    ConsumerSecret = ""
                }
            };
            //await auth.CredentialStore.ClearAsync();
            string twitterCallbackUrl = Request.GetDisplayUrl().Replace("Login", "CompleteLogin");
            return await auth.BeginAuthorizationAsync(new Uri(twitterCallbackUrl));
            //return View();
        }
        //GET: TwitterController/Details/5
        public async Task<ActionResult> CompleteLogin()
        {
            var auth = new MvcAuthorizer
            {
                CredentialStore = new SessionStateCredentialStore(HttpContext.Session)
            };

            await auth.CompleteAuthorizeAsync(new Uri(Request.GetDisplayUrl()));

            // This is how you access credentials after authorization.
            // The oauthToken and oauthTokenSecret do not expire.
            // You can use the userID to associate the credentials with the user.
            // You can save credentials any way you want - database, 
            //   isolated storage, etc. - it's up to you.
            // You can retrieve and load all 4 credentials on subsequent 
            //   queries to avoid the need to re-authorize.
            // When you've loaded all 4 credentials, LINQ to Twitter will let 
            //   you make queries without re-authorizing.
            //
            var credentials = auth.CredentialStore;
            string oauthtoken = credentials.OAuthToken;
            string oauthtokensecret = credentials.OAuthTokenSecret;
            string screenname = credentials.ScreenName;
            ulong userid = credentials.UserID;
            return View();
        }
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
                    ViewBag.Result = ex.Message;
                    Console.WriteLine(ex.Message);
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

        //// GET: TwitterController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: TwitterController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: TwitterController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: TwitterController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: TwitterController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: TwitterController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: TwitterController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
