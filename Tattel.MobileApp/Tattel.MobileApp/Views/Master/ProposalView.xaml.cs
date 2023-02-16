using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.ViewModels;
using Tattel.MobileApp.ViewModels.Master;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProposalView : ContentPage
    {
        ProposalViewModel _context;
        public ProposalView()
        {
            _context = new ProposalViewModel();
            InitializeComponent();
            this.BindingContext = _context;
            loadPage();
        }
        private async void loadPage()
        {
           await _context.LoadData();
           // this.BindingContext = _context;
            MyCollection.ItemsSource = _context.Items;
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var a = sender as Frame;
            var b = a.BindingContext as ProposalDTO;
            bool resp = await DisplayAlert("Do you want to accept this proposal", "This will start the chat", "Yes", "No");
            //string resp = await DisplayActionSheet("Do you to accept this proposal", "No", null, "yes");
            try
            {
                if (resp)
                {
                    Services.WebService web = new Services.WebService();
                    var temp = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);

                    await web.ConversationService.ApiConversationUpdateProposalResponsePutAsync(b.Id, true);

                    await DisplayAlert("Proposal Accepted", "Proceed to chat room", "Okay");
                    //await Navigation.PushAsync(new Chat());

                }
                else
                {
                    Services.WebService web = new Services.WebService();
                    var temp = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);

                    await web.ConversationService.ApiConversationUpdateProposalResponsePutAsync(b.Id, false);
                    await DisplayAlert("Proposal Declined", "You rejected the proposal", "Okay");
                    //await DisplayAlert("Proposal Declines", "Proposal has been declined", "Okay");

                }
            }

            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Okay");
            }
            //Navigation.PushAsync(new Profile.ProfilePage(b.SenderId, b.Id));
        }
    }
}