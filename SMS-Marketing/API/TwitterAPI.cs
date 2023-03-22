using LinqToTwitter;
using Microsoft.IdentityModel.Tokens;
namespace SMS_Marketing.API;
public class TwitterAPI
{
    //Properties
    public IFormFile? PostImage { get; set; } = null;
    public string Url { get; set; } = string.Empty;
    public TwitterContext? TwitterContext { get; set; }
    public Tweet? PostedTweet { get; set; } = null;
    public string TweetResult { get; set; } = string.Empty;

    //Private Members
    private string TwitterBody { get; set; } = string.Empty;
    private Stream FileStream { get; set; }
    private byte[] Bytes => new byte[FileStream.Length];
    public string MediaCategory { get; set; } = "tweet_image";

    public TwitterAPI(string boby, IFormFile? postImage, string url, TwitterContext? twitterContext)
    {
        TwitterBody = boby;
        if (TwitterBody.Length > 279)
        {
            TwitterBody = TwitterBody[..279];
        }
        if (postImage != null)
        {
            this.FileStream = postImage.OpenReadStream();
        }
        PostImage = postImage;
        Url = url;
        TwitterContext = twitterContext;
    }
    public async Task<bool> PostTweet()
    {
        try
        {
            if (TwitterBody.IsNullOrEmpty() || TwitterContext == null) throw new Exception("Invalid Twitter Post.");
            if (PostImage != null)
            {
                //Reads file
                FileStream.Read(Bytes, 0, (int)PostImage.Length);
                var imageUploads = new List<Task<Media>>
                {
                     TwitterContext.UploadMediaAsync(Bytes,PostImage.ContentType,MediaCategory),
                };

                //Uploads files
                await Task.WhenAll(imageUploads);
                List<string> mediaIds =
                            (from tsk in imageUploads
                             select tsk.Result.MediaID.ToString())
                            .ToList();

                //Posts tweets
                Tweet? tweet = await TwitterContext.TweetMediaAsync(TwitterBody, mediaIds);

                //Checks if tweet submitted
                if (tweet == null) throw new Exception("Could not post Tweet");
                PostedTweet = tweet;
                return true;
            }
            else
            {
                //When Image is not present
                var parameter = new Dictionary<string, string?>
                {
                    { "status", TwitterBody}
                };
                string queryString = "/statuses/update.json";

                //Posts tweet
                string? result = await TwitterContext.ExecuteRawAsync(queryString, parameter, HttpMethod.Post);
                if (result.IsNullOrEmpty()) throw new Exception("Could not post Tweet.");
                TweetResult = result;
                return true;
            }
        }
        catch (Exception ex)
        {
            Error.InitializeError("Posting Twitter", "400", ex.Message);
            Error.LogError();
            return false;
        }
    }
}
