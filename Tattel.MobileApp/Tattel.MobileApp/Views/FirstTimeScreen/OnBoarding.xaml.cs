using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.FirstTimeScreen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnBoarding : ContentPage
    {
        public OnBoarding()
        {
            InitializeComponent();
        }

        private void CarouselView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            string a = e.CurrentItem as string;
            if(a=="pic3.png")
            {
                lblName2.IsVisible = true;

            }
            else
            {

                lblName2.IsVisible = false;
            }
        }

        private async void Started_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FirstTimeScreenPage(), false);
        }
    }
}