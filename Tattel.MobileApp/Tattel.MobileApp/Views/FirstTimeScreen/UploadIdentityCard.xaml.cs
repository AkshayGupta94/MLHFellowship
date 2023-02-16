using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.Behavior;
using Tattel.MobileApp.Services;
using Tattel.MobileApp.ViewModels;
using Tattel.MobileApp.Views.Profile;
using Tattel.WebApiClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.FirstTimeScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadIdentityCard : ContentPage
    {
        MediaFile file;
        public UploadIdentityCard()
        {
            Settings.PageName = "Identity";
            InitializeComponent();
        }

        private async void btnStudent_Clicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return;

            //await DisplayAlert("File Location", file.Path, "OK");

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
               // file.Dispose();
                return stream;
            });
        }

        private async void btnNext_Clicked(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            UserResources userResources = new UserResources();
            string link = await userResources.UploadImage(file.GetStream(), file.Path);
            CachingService _cachingService; 
            _cachingService = new CachingService();
            var model = JsonConvert.DeserializeObject<ProfileViewModel>(_cachingService.GetCache("User"));
            model.LinkedInToken = link;
            _cachingService.PutCache("User", JsonConvert.SerializeObject(model));
            await Navigation.PushAsync(new EditUserProfilePage());
        }
    }
}