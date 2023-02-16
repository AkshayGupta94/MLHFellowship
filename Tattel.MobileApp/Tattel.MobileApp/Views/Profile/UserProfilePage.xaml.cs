using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Profile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserProfilePage : ContentPage
	{
		public UserProfilePage ()
		{
			InitializeComponent ();
		}

        private void BtnWERT_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.PushAsync(new Views.Profile.Popup.ChatRequestPopupPage(),false);
        }
    }
}