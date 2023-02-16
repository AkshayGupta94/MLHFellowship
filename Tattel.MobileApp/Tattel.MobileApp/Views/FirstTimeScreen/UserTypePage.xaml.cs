using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.Services;
using Tattel.MobileApp.ViewModels;
using Tattel.MobileApp.Views.Master;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.FirstTimeScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserTypePage : ContentPage
    {
        ProfileViewModel profileViewModel;
        CachingService cachingService; 
        public UserTypePage()
        {
            InitializeComponent();
            profileViewModel = new ProfileViewModel();
            cachingService = new CachingService();
        }

        private async void btnStudent_Clicked(object sender, EventArgs e)
        {
            profileViewModel.Type = 1;
            cachingService.PutCache("User", JsonConvert.SerializeObject(profileViewModel));
            await Navigation.PushAsync(new SignUpPage());
        }

        private async void btnProfessional_Clicked(object sender, EventArgs e)
        {
            profileViewModel.Type = 2;
            cachingService.PutCache("User", JsonConvert.SerializeObject(profileViewModel));
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}