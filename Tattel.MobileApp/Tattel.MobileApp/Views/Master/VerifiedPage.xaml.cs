using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Tattel.MobileApp.Behavior;
using Tattel.MobileApp.ViewModels;
using Tattel.MobileApp.Views.Profile;
using Xamarin.Forms;

namespace Tattel.MobileApp.Views.Master
{
    public partial class VerifiedPage : ContentPage
    {
        ProfileViewModel payload;
        public VerifiedPage(ProfileViewModel viewModel)
        {
            InitializeComponent();
            payload = viewModel;
            Settings.NumberVerifiedSetting = "yes";
            CheckExistingUser();
            
        }
        public async void CheckExistingUser()
        {
            Indicator.IsRunning = true;
            try
            {
                Services.WebService webService = new Services.WebService();
                var a = await webService.UserService.GetUserByEmailAsync(payload.EmailAddress);
                //if (a == null)
                //{
                //    Indicator.IsRunning = false;
                //    lblName2.IsEnabled = true;
                //    Application.Current.Properties["User"] = JsonConvert.SerializeObject(payload);
                //    await Application.Current.SavePropertiesAsync();
                //    Settings.UserProfileCreated = "yes";
                //}
                //else
                //{
                if (a != null)
                { 
                Indicator.IsRunning = false;
                ProfileViewModel profileViewModel = new ProfileViewModel(a);
                Application.Current.Properties["User"] = JsonConvert.SerializeObject(profileViewModel);
                await Application.Current.SavePropertiesAsync();
                Settings.UserProfileCreated = "yes";
                await DisplayAlert("User found", "The profile for existing user has been found", "Take me to main page");
                await Navigation.PushAsync(new Views.Profile.ProfilePage(), true);
                }
                //}
            }
            catch (Exception ex)
            {
                if(ex.Message== "Error calling GetUserByEmail: ")
                {
                    Indicator.IsRunning = false;
                    lblName2.IsEnabled = true;
                    Application.Current.Properties["User"] = JsonConvert.SerializeObject(payload);
                    await Application.Current.SavePropertiesAsync();
                    Settings.UserProfileCreated = "yes";
                }
                else
                await DisplayAlert("Error", ex.Message, "Okay");
                Indicator.IsRunning = false;
            }

        }

        async void Started_Clicked(object sender, System.EventArgs e)
        {
            // Check to see if user navigated off the page midway verification
            switch (App.IsInForeground)
            {
                case false:
                    return;

                case true:

                    if (payload.Type == 1)
                    {
                        await Navigation.PushAsync(new Views.FirstTimeScreen.UploadIdentityCard(), true);
                    }
                    else
                    {
                        await Navigation.PushAsync(new EditUserProfilePage());
                    }
                    break;
            }
        }
    }
}
