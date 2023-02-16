using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Master.Popup
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class notificationPopupPage : PopupPage
    {
		public notificationPopupPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await this.Content.ScaleTo(0, 0, Easing.Linear);
            await this.Content.ScaleTo(1, 500, Easing.BounceOut);
        }

        private async void Approve_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new Views.Profile.EditUserProfilePage());
        }

        
        private async void Decline_Clicked(object sender, EventArgs e)
        {

            //await Navigation.PushAsync(new Views.Profile.EditUserProfilePage());
        }
    }
}