using CoreGraphics;
using Foundation;
using MapKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tattel.MobileApp.CustomUI;
using Tattel.MobileApp.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;
using Tattel.MobileApp.Interfaces;
using Xamarin.Auth;

[assembly: Xamarin.Forms.Dependency(typeof(LinkedInAuth))]
namespace Tattel.MobileApp.iOS
{
    public class LinkedInAuth : ILinkedInAuth
    {
        void ILinkedInAuth.GetAuthToken()
        {
            string token = "";
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

            var UI = auth.GetUI();
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(UI, true, null);

        }
        private void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                string dd = e.Account.Username;
                var values = e.Account.Properties;
                var access_token = values["access_token"];
                MessagingCenter.Send<ILinkedInAuth, string>(this, "AuthSuccessful", access_token);

            }
        }
    }
}