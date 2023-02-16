using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Tattel.MobileApp.Views.FirstTimeScreen;
using Tattel.MobileApp.Behavior;
using Tattel.MobileApp.Views.Profile;
using Tattel.MobileApp.Interfaces;
using Tattel.MobileApp.Model;
using Tattel.MobileApp.Configuration;

using SimpleInjector;
using Xamarin.Essentials;
using Tattel.MobileApp.Views.SpalshScreen;
using Tattel.MobileApp.Views.Master;
using Newtonsoft.Json;
using Tattel.MobileApp.ViewModels;
using System;
using Tattel.MobileApp.Views.Chat;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Tattel.MobileApp
{
    public partial class App : Application
    {
        public static Container IoCContainer { get; set; } = new Container();

        public static bool IsInForeground { get; set; } = false;

        public string firstTime;
        
        public MapPage mapPage;
        public ChatPage chatPage;
        // Get and Set value to General settings
        public string PageOpen;
        public App()
        {
            try
            {
                PageOpen = "";

                InitializeComponent();
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzAxMTk4QDMxMzgyZTMyMmUzMGw2cjc2WUlyeEhNWkVRM1V1MEIvQmJVVkFMS1ZaUXR6d2QyTW1Qd05jKzg9");
                //Simple Injector (IoC for DI)

                IsInForeground = true;

                MainPage = new SplashScreenPage();
                IoCContainer.Register<IConfigurationClass>(() => new ConfigurationClass(), Lifestyle.Singleton);
                IoCContainer.Register<IMessageBird>(() => new MessageBirdClass(new ConfigurationClass()), Lifestyle.Singleton);
                //if(!Application.Current.Properties.ContainsKey("IsVerified"))
                //{
                //    Application.Current.Properties["IsVerified"] = "No";
                //   //    Application.Current.SavePropertiesAsync();
                //}

            }
            catch (Exception ex)
            { }


            //// Check is the app running for the first time
            //if (IsFirstTime == "yes")
            //{
            //    // if this is the first time, set it to "No" and load the
            //    // Main Page, which will show at the first time use
            //    //IsFirstTime = "no";
            //    MainPage = new NavigationPage(new FirstTimeScreenPage());
            //}
            //else
            //{
            //    // The phone number has already been verified so go to the profile page
            //    MainPage = new NavigationPage(new ProfilePage());
            //}
        }

        
        protected override void OnStart()
        {
            VersionTracking.Track();
        }

        protected override void OnSleep()
        {
           
            IsInForeground = false;
        }

        protected override void OnResume()
        {
            IsInForeground = true;
        }
    }
}
