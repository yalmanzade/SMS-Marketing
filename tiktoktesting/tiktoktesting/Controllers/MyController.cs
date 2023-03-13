using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
namespace tiktoktesting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyController : ControllerBase
    {
        private readonly ILogger<MyController> _logger;

        public MyController(ILogger<MyController> logger)
        {
            _logger = logger;
        }

        private const string CLIENT_KEY = "abc";
        private const string CLIENT_SECRET = "xyz";
        private const string SERVER_ENDPOINT_REDIRECT = "http://localhost:5000/callback";
        private const string API_BASE_URL = "https://open-api.tiktok.com";

        [HttpGet]
        [Route("oauth")]
        public IActionResult OAuth()
        {
            var csrfState = Guid.NewGuid().ToString();
            Response.Cookies.Append("csrfState", csrfState, new Microsoft.AspNetCore.Http.CookieOptions { MaxAge = TimeSpan.FromMinutes(1) });
            var url = "https://www.tiktok.com/auth/authorize/";
            url += "?client_key=" + CLIENT_KEY;
            url += "&scope=user.info.basic,video.upload";
            url += "&response_type=code";
            url += "&redirect_uri=" + SERVER_ENDPOINT_REDIRECT;
            url += "&state=" + csrfState;
            return Redirect(url);
        }

        [HttpGet]
        [Route("callback")]
        public async Task<IActionResult> Callback(string code, string state)
        {
            // exchange code for access token
            var accessToken = await GetAccessTokenAsync(code);

            // upload video
            var openId = await GetOpenIdAsync(accessToken);
            var filePath = "/Users/tiktok/Downloads/video.mp4";
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var result = await UploadVideoAsync(accessToken, openId, stream);
                return Ok(result);
            }
        }

        private async Task<string> GetAccessTokenAsync(string code)
        {
            var url = API_BASE_URL + "/oauth/access_token/";
            url += "?client_key=" + CLIENT_KEY;
            url += "&client_secret=" + CLIENT_SECRET;
            url += "&code=" + code;
            url += "&grant_type=authorization_code";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var responseString = await response.Content.ReadAsStringAsync();

                // parse access token from response
                // response should be in JSON format, with the access_token property containing the access token
                // e.g. { "access_token": "abc123", "expires_in": 3600 }
                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseString);
                var accessToken = jsonResponse.access_token;
                return accessToken;
            }
        }

        private async Task<string> GetOpenIdAsync(string accessToken)
        {
            var url = API_BASE_URL + "/oauth/openid/";
            url += "?access_token=" + accessToken;

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var responseString = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseString);
                var openId = jsonResponse.open_id;
                return openId;
            }
        }

        private async Task<string> UploadVideoAsync(string accessToken, string openId, Stream fileStream)
        {
            var url = API_BASE_URL + "/share/video/upload/";
            url += "?access_token=" + accessToken;
            url += "&open_id=" + openId;

            using (var httpClient = new HttpClient())
            using (var form = new MultipartFormDataContent())
            using (var binaryReader = new BinaryReader(fileStream))
            {
                var fileData = binaryReader.ReadBytes((int)fileStream.Length);
                var fileContent = new ByteArrayContent(fileData);
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "video",
                    FileName = "video.mp4"
                };
                form.Add(fileContent);
                var response = await httpClient.PostAsync(url, form);
                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
        }

    }
}