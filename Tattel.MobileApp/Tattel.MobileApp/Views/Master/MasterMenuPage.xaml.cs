using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.Behavior;
using Tattel.MobileApp.ViewModels;
using Tattel.MobileApp.Views.FirstTimeScreen;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Master
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterMenuPage : ContentPage
	{
        ProfileViewModel _context;
        public MasterMenuPage ()
		{
			InitializeComponent ();
            if (Device.RuntimePlatform == Device.iOS)
            {
                Title = "Tattel";
                this.IconImageSource = "iconMenu.png";
            }
            _context = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);
           
            Uri uri = new Uri(_context.ProfileImageLink);
            var ab = UriImageSource.FromUri(uri);
            ProfilePic.Source = ab;
           
            lblUserName.Text = _context.Name;
            lblEmail.Text = _context.EmailAddress;
        }

        private async void GrdHome_Tapped(object sender, EventArgs e)
        {
            //map page
            Utilities.Common.MasterPage.Detail = new NavigationPage(new Views.Chat.MapPage());
            Utilities.Common.MasterPage.IsGestureEnabled = true;
            Utilities.Common.MasterPage.IsPresented = false;
            await grdHome.ScaleTo(1.2, 300, Easing.SpringIn);
            await grdHome.ScaleTo(1, 300, Easing.Linear);
        }

        private async void GrdNotice_Tapped(object sender, EventArgs e)
        {
            //proposals
            Utilities.Common.MasterPage.Detail = new NavigationPage(new Views.Master.ProposalView());
            Utilities.Common.MasterPage.IsGestureEnabled = true;
            Utilities.Common.MasterPage.IsPresented = false;
            await grdNotice.ScaleTo(1.2, 300, Easing.SpringIn);
            await grdNotice.ScaleTo(1, 300, Easing.Linear);
        }

        private async void GrdChat_Tapped(object sender, EventArgs e)
        {
            await grdChat.ScaleTo(1.2, 300, Easing.SpringIn);
            await grdChat.ScaleTo(1, 300, Easing.Linear);
            Utilities.Common.MasterPage.Detail = new NavigationPage(new Views.Master.ChatListView());
            Utilities.Common.MasterPage.IsGestureEnabled = true;
            Utilities.Common.MasterPage.IsPresented = false;
        }

        private async void GrdUser_Tapped(object sender, EventArgs e)
        {
            await grdUser.ScaleTo(1.2, 300, Easing.SpringIn);
            await grdUser.ScaleTo(1, 300, Easing.Linear);
     
            Utilities.Common.MasterPage.Detail = new NavigationPage(new Views.Profile.ProfilePage());
            Utilities.Common.MasterPage.IsGestureEnabled = true;
            Utilities.Common.MasterPage.IsPresented = false;
        }

        private async void GrdLogout_Tapped(object sender, EventArgs e)
        {
            await grdLogout.ScaleTo(1.2, 300, Easing.SpringIn);

            await grdLogout.ScaleTo(1, 300, Easing.Linear);

            Settings.UserProfileCreated = "no";
            Settings.NumberVerifiedSetting = "no";
            Settings.PageName = "Logout";
            Application.Current.MainPage = new NavigationPage(new FirstTimeScreenPage());
        }

        private async void GrdInfo_Tapped(object sender, EventArgs e)
        {
            //about us
            await grdInfo.ScaleTo(1.2, 300, Easing.SpringIn);
            await grdInfo.ScaleTo(1, 300, Easing.Linear);
            await Browser.OpenAsync(new Uri("https://www.tattel.co.uk/"), BrowserLaunchMode.SystemPreferred);
            //Utilities.Common.MasterPage.Detail = new NavigationPage(new Views.Chat.MapPage());
            //Utilities.Common.MasterPage.IsGestureEnabled = true;
            //Utilities.Common.MasterPage.IsPresented = false;
        }

        private async void GrdSuccess_Tapped(object sender, EventArgs e)
        {
            await grdSuccess.ScaleTo(1.2, 300, Easing.SpringIn);
            await grdSuccess.ScaleTo(1, 300, Easing.Linear);
            await Browser.OpenAsync(new Uri("https://www.tattel.co.uk/privacy-policy"), BrowserLaunchMode.SystemPreferred);

        }
    }
}