
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HTTPCORETESTING
{
    class Program
    {
        static void Main(string[] args)
        {
            var accessToken = "EABXHnZAodiekBABCnjZA9v2TXHG6ERk6ycuvpVWovgzBdZC8UnEBg9Crw1W0lzXppMIxOVlqScZALwAPhiPBdHHLgwqms0XXZBJMHvQpjhqArulRQplmd6zEfTiokNkgvdm87ZCpbWtSsIXs71kd6fZAtAhBvqacXfv4WZANNhx9HELXOCHqFhOW";
            var pageId = "115126374823354";
            var message = "Hello from C#!";

            var url = $"https://graph.facebook.com/v16.0/{pageId}/feed?access_token={accessToken}";
            var data = $"message={message}";
            var dataBytes = Encoding.UTF8.GetBytes(data);

            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url, new ByteArrayContent(dataBytes)).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Post was successful.");
                }
                else
                {
                    Console.WriteLine("Post was unsuccessful. Response code: " + (int)response.StatusCode);
                }
            }
        }
    }
}