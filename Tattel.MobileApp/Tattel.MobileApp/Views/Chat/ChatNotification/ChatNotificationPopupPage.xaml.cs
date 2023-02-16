using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Chat.ChatNotification
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatNotificationPopupPage : PopupPage
    {
		public ChatNotificationPopupPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await this.Content.ScaleTo(0, 0, Easing.Linear);
            await this.Content.ScaleTo(1, 500, Easing.BounceOut);
        }

        private void BtnOk_Clicked(object sender, EventArgs e)
        {

        }
    }
}