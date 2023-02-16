using System;

using Tattel.MobileApp.Interfaces;

using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms;

using MessageBird.Exceptions;
using Tattel.MobileApp.ViewModels;
using Tattel.MobileApp.Services;
using Newtonsoft.Json;
using Tattel.MobileApp.Behavior;

namespace Tattel.MobileApp.Views.Master
{
    public partial class VerifyCodePage : ContentPage
    {
        protected IMessageBird messageBird;
        ProfileViewModel viewModel;
        CachingService _cachingService;

        public VerifyCodePage()
        {
            Settings.PageName = "Verify";
            InitializeComponent();
            _cachingService = new CachingService();
            viewModel = JsonConvert.DeserializeObject<ProfileViewModel>(_cachingService.GetCache("User"));
        }
        public VerifyCodePage(ProfileViewModel payload)
        {
            InitializeComponent();
            viewModel = payload;
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            //Using Simple Injector to retrieve our Objects
            //messageBird = App.IoCContainer.GetInstance<IMessageBird>();
        }


        private async void BtnVerify_Clicked(object sender, EventArgs e)
        {
            // Check if verification code is empty
            if (txtCode.Text == null)
            {
                //Popup - Using 'Page' Class Method
                await DisplayAlert("Please enter your verification code", "Code cannot be blank.", "OK");
                return;
            }
            else
            {
                try
                {
                    //messageBird.VerifyOTP(txtCode.Text);
                    WebService webService = new WebService();
                    var check = await webService.UserService.VerifyUserAsync(viewModel.EmailAddress, txtCode.Text);

                    // Check to see if user navigated off the page midway verification
                    switch (check)
                    {
                        case false:
                            await DisplayAlert("Please Check your verification code", "Code Entered is not correct.", "OK");

                            return;

                        case true:
                            await Navigation.PushAsync(new Views.Master.VerifiedPage(viewModel), false);
                            break;
                    }
                }
                catch(ErrorException errorException)
                {
                    Console.WriteLine(errorException.Message);
                }
            }
        }
    }
}
