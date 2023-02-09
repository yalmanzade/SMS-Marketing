using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PostFacebook.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string accessToken, string pageId, string message)
        {
            var url = $"https://graph.facebook.com/v16.0/{pageId}/feed?access_token={accessToken}";
            var data = $"message={message}";
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, new ByteArrayContent(dataBytes));

                if (response.IsSuccessStatusCode)
                {
                    TempData["Result"] = "Post was successful.";
                }
                else
                {
                    TempData["Result"] = "Post was unsuccessful.";
                }
            }

            return RedirectToAction("Index");
        }

    }
}
