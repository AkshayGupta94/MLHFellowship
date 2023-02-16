using System;
using Lottie.Forms.Droid;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Tattel.MobileApp.Interfaces;
using Xamarin.Forms;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Android.Gms.Common;
using Tattel.MobileApp.Model;
using Xamarin.Essentials;
using Android;
using Android.Content;

namespace Tattel.MobileApp.Droid
{
    [Activity(Label = "Tattel", Icon = "@mipmap/icon", MainLauncher = true, Theme = "@style/MainTheme",  ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        const int RequestLocationId = 0;
        public static Context Context;
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;
        readonly string[] LocationPermissions =
        {
    Manifest.Permission.AccessCoarseLocation,
    Manifest.Permission.AccessFineLocation
};

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            Context = this;
            base.OnCreate(savedInstanceState);
            
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            AnimationViewRenderer.Init();
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            Window.SetSoftInputMode(Android.Views.SoftInput.AdjustResize);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            IsPlayServicesAvailable();
            Xamarin.FormsMaps.Init(this, savedInstanceState);

            CreateNotificationChannel();
            var av = FirebaseInstanceId.Instance.Token;
            LoadApplication(new App());
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("Fcmtocken"))
            {
                Xamarin.Forms.Application.Current.Properties["Fcmtocken"] = FirebaseInstanceId.Instance.Token;
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }
            else 
            {
                Xamarin.Forms.Application.Current.Properties.Add("Fcmtocken",FirebaseInstanceId.Instance.Token);
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }
            
            loadPermissions();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public async void loadPermissions()
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
           
        }
        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }
        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }
                else
                {
                    // Permissions already granted - display a message.
                }
            }
        }
        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                { }
                else
                {
                    
                    Finish();
                }
                return false;
            }
            else
            {
                
                return true;
            }
        }
    }
}