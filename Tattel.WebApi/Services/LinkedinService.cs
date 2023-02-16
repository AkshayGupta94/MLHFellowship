using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Tattel.WebApi.DataModels;

namespace Tattel.WebApi.Services
{
    public static class LinkedinService
    {
        public async static Task<string> GetUserEmail(string access_token)
        {
            var request = HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v2/emailAddress?q=members&projection=(elements*(handle~))"));
            request.ContentType = "application /json";
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + access_token);
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return null;
                    }
                    var result = JsonConvert.DeserializeObject<LinkedInEmailResponse>(content);
                    return result.elements[0].Handle.emailAddress;
                }
            }
        }

        public async static Task<LinkedInProfileResponse> GetUserProfile(string access_token)
        {
            var request = HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~:playableStreams))"));
            request.ContentType = "application /json";
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + access_token);
            try
            {
                using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        var content = reader.ReadToEnd();
                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            return JsonConvert.DeserializeObject<LinkedInProfileResponse>(content);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                return null;
            }
            return null;
        }
    }
}
