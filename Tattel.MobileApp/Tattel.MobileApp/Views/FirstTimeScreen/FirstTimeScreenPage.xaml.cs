using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.Views.Master.Popup;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace Tattel.MobileApp.Views.FirstTimeScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstTimeScreenPage : ContentPage
    {
       
        public FirstTimeScreenPage()
        {
            InitializeComponent();
        }

        /*
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(5 * 1000);
            App.Current.MainPage = new Views.Master.MasterMainPageDetail();
        }
        */

        async void PrivacyGesture_Tapped(object sender, System.EventArgs e)
        {

            //Open Browser
            await Browser.OpenAsync(new Uri("https://www.tattel.co.uk/privacy-policy"), BrowserLaunchMode.SystemPreferred);


        }


        async void TermsGesture_Tapped(object sender, System.EventArgs e)
        {
            //Open Browser
            await Browser.OpenAsync(new Uri("https://www.tattel.co.uk/privacy-policy"), BrowserLaunchMode.SystemPreferred);

        }


        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Views.FirstTimeScreen.UserTypePage());
            //await Navigation.PushAsync(new Views.Master.SignUpPage(), false);
        }
    }
}