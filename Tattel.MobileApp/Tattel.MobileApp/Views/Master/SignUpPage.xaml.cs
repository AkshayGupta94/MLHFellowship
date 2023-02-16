using System;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

using Tattel.MobileApp.Interfaces;


using MessageBird.Exceptions;
using System.Reflection;
using System.IO;
using Tattel.MobileApp.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tattel.MobileApp.ViewModels;
using Tattel.WebApiClient.Model;
using Tattel.MobileApp.Services;

namespace Tattel.MobileApp.Views.Master
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignUpPage : ContentPage
	{
        protected IMessageBird messageBird;
        string selectedCode;
        User user;
        private const string Namespace = "Tattel.MobileApp";
        private const string FileName = "countries.json";
        public ObservableCollection<Countries> _countries { get; set; }
        public SignUpPage ()
		{
			InitializeComponent ();
            CachingService cachingService = new CachingService();

            cachingService = new CachingService();
            ProfileViewModel profileViewModel = JsonConvert.DeserializeObject<ProfileViewModel>(cachingService.GetCache("User"));
            if (profileViewModel.Type == 2)
            {
                lblName1.Text = "organisation email";
            }
             user = new User();
        }


        private async void BtnNext_Clicked(object sender, EventArgs e)
        {
            bool resp = await DisplayAlert("Age Warning", "To use this app you must be 18 years or older. By continuing you accept that you meet the minimum age requirement. If you are not 18 years old please decline", "Accept", "Decline");
            CachingService cachingService = new CachingService();
            if (resp)
            {

                // Check if phone number is empty
                if (txtNumber.Text == null)
                {
                    //Popup - Using 'Page' Class Method
                    await DisplayAlert("Please enter a valid Email Address", "Email Address cannot be blank.", "OK");
                    return;
                }
                // Validating mobile number is 10 digits 
                if (txtNumber.Text.Split('@')[1].Contains("ac.uk"))
                {
                    //Popup - Using 'Page' Class Method
                    await DisplayAlert("Please enter a school Email Address", "Email address needs to end with ac.uk", "OK");
                    return;
                }
                
                else
                {
                    try
                    {
                        cachingService = new CachingService();
                        ProfileViewModel profileViewModel = JsonConvert.DeserializeObject<ProfileViewModel>(cachingService.GetCache("User"));
                        // Assigning the phone number to the user
                        profileViewModel.EmailAddress = txtNumber.Text;
                        //user.UserType = 1;
                       // ProfileViewModel profileViewModel = new ProfileViewModel(user);
                       
                        cachingService.PutCache("User", JsonConvert.SerializeObject(profileViewModel));
                        //Todo: API call for emailU
                        WebService webService = new WebService();
                        await webService.UserService.ApiUserSendVerifyOtpPostAsync(profileViewModel.EmailAddress);
                        await Navigation.PushAsync(new VerifyCodePage());
                       
                    }
                    catch (ErrorException errorException)
                    {
                        Console.WriteLine(errorException.Message);
                    }
                }
            }
        }

        private void Countries_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
           // var a = Countries.SelectedItem as Countries;
            //
            //selectedCode = a.dial_code;
        }
    }
}