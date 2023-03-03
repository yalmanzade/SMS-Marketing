using Microsoft.AspNetCore.Mvc;

namespace tiktoktesting.Controllers
{
    public class LoginController : Controller
    {
        private const string CLIENT_KEY = "abc";

        [HttpGet]
        public IActionResult Index()
        {
            var csrfState = Guid.NewGuid().ToString();
            HttpContext.Response.Cookies.Append("csrfState", csrfState, new CookieOptions { MaxAge = TimeSpan.FromMinutes(1) });

            var serverEndpoint = "{SERVER_ENDPOINT_OAUTH}";
            return Redirect(serverEndpoint);
        }
    }
}
