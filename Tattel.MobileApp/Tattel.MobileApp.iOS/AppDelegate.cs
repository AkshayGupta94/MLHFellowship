using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.CloudMessaging;
using Foundation;
using KeyboardOverlap.Forms.Plugin.iOSUnified;
using Lottie.Forms.iOS.Renderers;
using Tattel.MobileApp.Interfaces;
using Tattel.MobileApp.Model;
using UIKit;
using UserNotifications;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Tattel.MobileApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate, IMessagingDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            UITabBar.Appearance.TintColor = UIColor.FromRGB(255, 255, 255);
            UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(69, 79, 99);
            UINavigationBar.Appearance.TintColor = UIColor.White;
            // To change Title Text colors to white here
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() { TextColor = UIColor.White });
            var attribute = new UITextAttributes();

            //UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;

            /*
            UIView statusBar = new UIView(UIApplication.SharedApplication.KeyWindow.WindowScene.StatusBarManager.StatusBarFrame);
            //statusBar.BackgroundColor = Color.FromHex("#ffffff").ToUIColor();
            UIApplication.SharedApplication.KeyWindow.AddSubview(statusBar);

            if (statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
            {
                statusBar.BackgroundColor = Color.FromHex("#ffffff").ToUIColor();
                //statusBar.TintColor = Color.FromHex("#ffffff").ToUIColor();
            }
            */

            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            LoadApplication(new App());
            KeyboardOverlapRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
            new Syncfusion.XForms.iOS.ComboBox.SfComboBoxRenderer();
            AnimationViewRenderer.Init();
            Firebase.Core.App.Configure();
            RegisterForRemoteNotifications();
            CheckAndRequestLocationPermission();
            Messaging.SharedInstance.Delegate = this;
            if (Messaging.SharedInstance.FcmToken!=null)
            {
                Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = Messaging.SharedInstance.FcmToken ?? "";

                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }
            
           
            if (UNUserNotificationCenter.Current != null)
            {
                UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();
            }
           


            return base.FinishedLaunching(app, options);
        }

        private void RegisterForRemoteNotifications()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.Delegate = this;
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, async (granted, error) =>
                {
                    Console.WriteLine(granted);
                    await System.Threading.Tasks.Task.Delay(500);
                });
                Messaging.SharedInstance.Delegate = this;
            }
            else
            {
                // iOS 9 or before
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }
            UIApplication.SharedApplication.RegisterForRemoteNotifications();


            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;




        }

        public async Task<PermissionStatus> CheckAndRequestLocationPermission()
        {
             
             PermissionStatus   status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            

            // Additionally could prompt the user to turn on in settings

            return status;
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {

            Messaging.SharedInstance.ApnsToken = deviceToken;

        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            //base.FailedToRegisterForRemoteNotifications(application, error)
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)

        {

            completionHandler(UIBackgroundFetchResult.NewData);

        }

        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {

            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = Messaging.SharedInstance.FcmToken ?? "";

            Xamarin.Forms.Application.Current.SavePropertiesAsync();

            System.Diagnostics.Debug.WriteLine("######Token######  :  {fcmToken}");

            Console.WriteLine(fcmToken);

        }

        public void DidRefreshRegistrationToken(Messaging messaging, string fcmToken)
        {
            Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = Messaging.SharedInstance.FcmToken ?? "";

            Xamarin.Forms.Application.Current.SavePropertiesAsync();
        }
    }
}
