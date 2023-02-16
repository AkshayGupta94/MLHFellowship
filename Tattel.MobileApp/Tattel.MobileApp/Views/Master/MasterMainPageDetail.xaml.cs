using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.Views.Chat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterMainPageDetail : MasterDetailPage
    {
        public MasterMainPageDetail()
        {
            try
            {
                //this.Master = new NavigationPage(new MasterDetailPage
                //{
                //    Title = "Menu"
                //});
                ////this.Master = new MasterDetailPage { Title = "Menu" };
                //this.Detail = new MapPage();
                InitializeComponent();
               
                Utilities.Common.MasterPage = this;
                // Utilities.Common.MasterPage.Detail = new NavigationPage(new Views.Master.SignUpPage());
                MasterBehavior = MasterBehavior.Popover;
                //Utilities.Common.MasterPage.IsGestureEnabled = false;
                //Utilities.Common.MasterPage.IsPresented = false;
            }
            catch (Exception ex)
            {
                var exp = ex.Message;
            }
        }
    }
}