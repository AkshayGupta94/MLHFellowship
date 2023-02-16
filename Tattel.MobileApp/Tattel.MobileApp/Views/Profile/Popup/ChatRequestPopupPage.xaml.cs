using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Profile.Popup
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatRequestPopupPage : PopupPage
    {
		public ChatRequestPopupPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await this.Content.ScaleTo(0, 0, Easing.Linear);
            await this.Content.ScaleTo(1, 500, Easing.BounceOut);
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {

        }

        private async void BtnOk_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Chat.MapPage());

        }
    }
}