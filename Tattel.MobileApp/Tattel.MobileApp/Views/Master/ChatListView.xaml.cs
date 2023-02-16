using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.ViewModels.Master;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatListView : ContentPage
    {
        ProposalViewModel _context;
        public ChatListView()
        {
            _context = new ProposalViewModel();
            InitializeComponent();
            this.BindingContext = _context;
            loadPage();
        }
        private async void loadPage()
        {
            try
            {
                await _context.LoadChatData();
                // this.BindingContext = _context;
                MyCollection.ItemsSource = _context.Items;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Okay");
                    
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var a = sender as Frame;
            var selectedItem = a.BindingContext as ProposalDTO;
            Navigation.PushAsync(new Chat.ChatPage(selectedItem));
        }
    }
}