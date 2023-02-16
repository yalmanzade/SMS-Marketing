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
        public async Task<IActionResult> Index(string accessToken, string pageId, string message, IFormFile imageFile)
        {
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
                        TempData["Result"] = "Post was successful.";
                    }
                    else
                    {
                        TempData["Result"] = "Post was unsuccessful.";
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
                            TempData["Result"] = "Post was successful.";
                        }
                        else
                        {
                            TempData["Result"] = "Post was unsuccessful.";
                        }
                    }
                }
            }

            return RedirectToAction("Index");
        }

    }
}
