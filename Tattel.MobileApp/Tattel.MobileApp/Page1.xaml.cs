using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page1 : ContentPage
	{
		public Page1 ()
		{
			InitializeComponent ();
		}

        private void Grd2_Focused(object sender, FocusEventArgs e)
        {
            grd2.Focus();
            grd.Margin = new Thickness(0, 0, 0, 0);
        }

        private void Grd2_Unfocused(object sender, FocusEventArgs e)
        {
            grd2.Unfocus();
            grd.Margin = new Thickness(0, 0, 0, 0);
        }
    }
}