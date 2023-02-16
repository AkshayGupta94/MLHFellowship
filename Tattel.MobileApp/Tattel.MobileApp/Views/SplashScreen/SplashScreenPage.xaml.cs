using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tattel.MobileApp.Behavior;
using Tattel.MobileApp.Views.FirstTimeScreen;
using Tattel.MobileApp.Views.Master;
using Tattel.MobileApp.Views.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.SpalshScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreenPage : ContentPage
    {
        public string IsFirstTime
        {
            get { return Settings.FirstTime; }
            set
            {
                if (Settings.FirstTime == value)
                    return;
                Settings.FirstTime = value;
                OnPropertyChanged();
            }
        }

        // Get and Set value to Number Verified setting
        public string IsPhoneNumberVerified
        {
            get { return Settings.NumberVerifiedSetting; }
            set
            {
                if (Settings.NumberVerifiedSetting == value)
                    return;
                Settings.NumberVerifiedSetting = value;
                OnPropertyChanged();
            }
        }

        public string PageName
        {
            get { return Settings.PageName; }
            set
            {
                if (Settings.PageName == value)
                    return;
                Settings.PageName = value;
                OnPropertyChanged();
            }
        }

        public string IsUserCreated
        {
            get { return Settings.UserProfileCreated; }
            set
            {
                if (Settings.UserProfileCreated == value)
                    return;
                Settings.UserProfileCreated = value;
                OnPropertyChanged();
            }
        }

        public SplashScreenPage()
        {
            InitializeComponent();
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            if(currentTheme== OSAppTheme.Dark)
            {
                DisplayAlert("Warning", "The app is best viewed in light mode", "Ok");
            }
            Device.StartTimer(new TimeSpan(0, 0, 3), () =>
               {
                   switch(PageName)
                   {
                       case "Profile":
                           Application.Current.MainPage = new NavigationPage(new ProfilePage());
                           break;
                       case "Verify":
                           Application.Current.MainPage = new NavigationPage(new VerifyCodePage());
                           break;
                       case "Identity":
                           Application.Current.MainPage = new NavigationPage(new UploadIdentityCard());
                           break;
                       case "EditProfile":
                           Application.Current.MainPage = new NavigationPage(new EditUserProfilePage());
                           break;
                       default:
                           Application.Current.MainPage = new NavigationPage(new OnBoarding());
                           break;
                   }
                  
                   return false;
               });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //animationView.ImageAssetsFolder = "logo";
            //animationView.Duration = TimeSpan.FromSeconds(10);
            //animationView.Play();

            //await Task.Delay(5 * 1000);
            

        }

        private Page newFirstTimeScreenPage()
        {
            throw new NotImplementedException();
        }
    }
}