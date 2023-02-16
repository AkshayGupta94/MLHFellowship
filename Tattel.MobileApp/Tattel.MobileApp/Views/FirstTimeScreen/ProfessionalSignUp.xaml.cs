using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.Behavior;
using Tattel.MobileApp.Interfaces;
using Tattel.MobileApp.Model;
using Tattel.MobileApp.Services;
using Tattel.MobileApp.ViewModels;
using Tattel.MobileApp.Views.Profile;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.FirstTimeScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfessionalSignUp : ContentPage
    {
        public ProfileViewModel profileViewModel;
        public ProfessionalSignUp()
        {
            profileViewModel = new ProfileViewModel { Type = 2 };
            InitializeComponent();
            MessagingCenter.Subscribe<ILinkedInAuth, string>(this, "AuthSuccessful", async (sender, arg) =>
            {
            string access_token = arg;
                try
                {
                    WebService webService = new WebService();
                    var user = await webService.UserService.ApiUserAddPostAsync(new WebApiClient.Model.User { LinkdienToken=arg });
                    ProfileViewModel profileViewModel = new ProfileViewModel(user);
                /*
                    //    var request = HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v2/emailAddress?q=members&projection=(elements*(handle~))"));
                //request.ContentType = "application /json";
                //request.Method = "GET";
                //request.PreAuthenticate = true;
                //request.Headers.Add("Authorization", "Bearer " + access_token);
                //using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                //{
                //    System.Console.Out.WriteLine("Stautus Code is: {0}", response.StatusCode);

                //    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                //    {
                //        var content = reader.ReadToEnd();
                //        if (!string.IsNullOrWhiteSpace(content))
                //        {
                //            System.Console.Out.WriteLine(content);
                //        }
                //        var result = JsonConvert.DeserializeObject<LinkedInEmailResponse>(content);
                //        string email = result.elements[0].Handle.emailAddress;
                //        profileViewModel.EmailAddress = email;
                //    }
                //}
                //request = HttpWebRequest.Create(string.Format(@"https://api.linkedin.com/v2/me?projection=(id,firstName,lastName,profilePicture(displayImage~:playableStreams))"));
                //request.ContentType = "application /json";
                //request.Method = "GET";
                //request.PreAuthenticate = true;
                //request.Headers.Add("Authorization", "Bearer " + access_token);
                //    LinkedInProfileResponse linkedInProfileResponse;
                //using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                //{
                //    System.Console.Out.WriteLine("Stautus Code is: {0}", response.StatusCode);

                //    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                //    {
                //        var content = reader.ReadToEnd();
                //        if (!string.IsNullOrWhiteSpace(content))
                //        {
                //            System.Console.Out.WriteLine(content);
                //                profileViewModel.Age = content;
                //        }
                //        linkedInProfileResponse = JsonConvert.DeserializeObject<LinkedInProfileResponse>(content);
                //        System.Console.Out.WriteLine(linkedInProfileResponse.firstName.localized.en_US + " " + linkedInProfileResponse.lastName.localized.en_US + " " + linkedInProfileResponse.profilePicture.displayImage.Split(':')[3]);
                //        profileViewModel.Name = linkedInProfileResponse.firstName.localized.en_US + " " + linkedInProfileResponse.lastName.localized.en_US;
                //        profileViewModel.ProfileImageLink = linkedInProfileResponse.profilePicture.DisplayImage.elements[0].identifiers[0].identifier;




                //        }
                //    }
                    //request = HttpWebRequest.Create(linkedInProfileResponse.profilePicture.DisplayImage.elements[0].identifiers[0].identifier);
                    //request.ContentType = "application /json";
                    //request.Method = "GET";
                    //request.PreAuthenticate = true;
                    //request.Headers.Add("Authorization", "Bearer " + access_token);
                    //using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
                    //{
                    //    System.Console.Out.WriteLine("Stautus Code is: {0}", response.StatusCode);

                    //    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    //    {
                    //        var bytes = default(byte[]);
                    //        using (var memstream = new MemoryStream())
                    //        {
                    //            reader.BaseStream.CopyTo(memstream);
                    //            bytes = memstream.ToArray();
                    //        }

                    //        profileViewModel.LinkedInProfileImage = new Image();

                    //        profileViewModel.LinkedInProfileImage.Source = ImageSource.FromStream(()=> new MemoryStream(bytes));
                    //        //Test.Source = ImageSource.FromStream(() => new MemoryStream(bytes));
                            

                    //    }
                    //}*/
                    CachingService _cachingService = new CachingService();
                    _cachingService.PutCache("User", JsonConvert.SerializeObject(profileViewModel));
                    Settings.NumberVerifiedSetting = "yes";
                    await Navigation.PushAsync(new EditUserProfilePage());
                    //webService.UserService.ApiUserAddPostAsync(new WebApiClient.Model.User { })
                }


                catch (Exception exx)
                {
                    System.Console.WriteLine(exx.ToString());
                }
            });
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
             DependencyService.Get<ILinkedInAuth>().GetAuthToken();
        }
    }
}