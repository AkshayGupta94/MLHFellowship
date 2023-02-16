using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Tattel.MobileApp.Droid;
using Tattel.MobileApp.Interfaces;
using Tattel.MobileApp.Views.FirstTimeScreen;
using Xamarin.Auth;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(LinkedInAuth))]
namespace Tattel.MobileApp.Droid
{
    public class LinkedInAuth : ILinkedInAuth
    {
        void ILinkedInAuth.GetAuthToken()
        {
            string token="";
            var auth = new OAuth2Authenticator(
                 clientId: "78ipyo7kqxwm7m",
                 clientSecret: "o9MQHeBA06ftbYmA",
                 scope: "r_liteprofile,r_emailaddress",
                 authorizeUrl: new Uri("https://www.linkedin.com/uas/oauth2/authorization"),
                   redirectUrl: new Uri("http://www.google.com/"),
                accessTokenUrl: new Uri("https://www.linkedin.com/uas/oauth2/accessToken")
                );
            
            auth.AllowCancel = true;
            auth.Completed += Auth_Completed;
            var UI = auth.GetUI(Android.App.Application.Context);
            MainActivity.Context?.StartActivity(UI);


        }

        private async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                string dd = e.Account.Username;
                var values = e.Account.Properties;
                var access_token = values["access_token"];
                MessagingCenter.Send<ILinkedInAuth, string>(this, "AuthSuccessful", access_token);
                string token = access_token;
                try
                {

                    var request = HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v2/emailAddress?q=members&projection=(elements*(handle~))"));
                    request.ContentType = "application /json";
                    request.Method = "GET";
                    request.PreAuthenticate = true;
                    request.Headers.Add("Authorization", "Bearer " + access_token);
                    using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                    {
                        System.Console.Out.WriteLine("Stautus Code is: {0}", response.StatusCode);

                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            var content = reader.ReadToEnd();
                            if (!string.IsNullOrWhiteSpace(content))
                            {
                                System.Console.Out.WriteLine(content);
                            }
                            var result = JsonConvert.DeserializeObject<LinkedInResponse>(content);
                            string email = result.elements[0].Handle.emailAddress;

                        }
                    }
                }
                catch (Exception exx)
                {
                    System.Console.WriteLine(exx.ToString());
                }

            }
        }
    }
}