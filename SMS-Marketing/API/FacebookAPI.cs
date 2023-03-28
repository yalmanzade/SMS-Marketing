using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SMS_Marketing.Areas.Identity.Data;
using SMS_Marketing.Data;
using SMS_Marketing.Models;
using System.Text;

namespace SMS_Marketing.API
{
    public class FacebookAPI
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAuthDbContext _authContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public FacebookAPI(ApplicationDbContext context, UserAuthDbContext authDbContext,
                                         UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _authContext = authDbContext;
        }
        public async Task<String> PostToFacebook(string message, IFormFile? imageFile, int? id)
        {
            FacebookAuth? facebookAuth = _context.FacebookAuth
                                               .Where(x => x.OrganizationId == id)
                                               .FirstOrDefault();

            if (facebookAuth.AccessToken == null || facebookAuth.AppId == null) return "false";
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
                        string result;
                        result = "Post was successful.";
                        return result;
                    }
                    else
                    {
                        string result;
                        result = "Post was unsuccessful.";
                        return result;
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
                            string result = "Post was successful.";
                            return result;
                        }
                        else
                        {
                            string result = "Post was unsuccessful.";
                            return result;
                        }
                    }
                }
            }
        }
    }
}
