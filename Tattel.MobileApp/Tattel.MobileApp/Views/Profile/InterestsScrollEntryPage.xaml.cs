using Rg.Plugins.Popup.Pages;
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
	public partial class InterestsScrollEntryPage : PopupPage
    {
		public InterestsScrollEntryPage ()
		{
			InitializeComponent ();
           
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await frmMain.TranslateTo(0, 500, 0, Easing.Linear);
            await frmMain.TranslateTo(0, 0, 500, Easing.Linear);
        }

        private async void LblOk_Tapped(object sender, EventArgs e)
        {
            await lblOk.ScaleTo(1.2, 200, Easing.SpringIn);
            await lblOk.ScaleTo(1, 200, Easing.Linear);
            await frmMain.TranslateTo(0, 500, 500, Easing.Linear);
            PopupNavigation.PopAsync(false);
        }
    }
}