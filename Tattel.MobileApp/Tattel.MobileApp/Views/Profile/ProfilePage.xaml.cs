using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.Behavior;
using Tattel.MobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Profile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        ProfileViewModel _context;
        string type;
        string proposalId;
        public ProfilePage()
        {
            try
            {

                Settings.PageName = "Profile";
                _context = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);
                type = "";

                InitializeComponent();
                this.BindingContext = _context;
               
                Services.WebService webService = new Services.WebService();
                webService.UserService.ApiUserUpdateTokenUpdateTokenPut(_context.Id, Application.Current.Properties["Fcmtocken"].ToString());
                Uri uri = new Uri(_context.ProfileImageLink);
                var a = UriImageSource.FromUri(uri);
                ProfileImg.Source = a;
                if(_context.Type==1)
                {
                    lblName1.Text = "University/ College: ";
                    lblName.Text = "Aspiring job role";
                   // lblLocation.Text = "Student";
                }

            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Okay");
            }

        }
        public ProfilePage (ProfileViewModel payload)
		{
            _context = payload;
            type = "";
			InitializeComponent ();
            this.BindingContext = _context;


        }
        public ProfilePage(string id)
        {
            Services.WebService web = new Services.WebService();
            var a = web.UserService.GetUser(id);
            _context = new ProfileViewModel(a);
            InitializeComponent();
            this.BindingContext = _context;
            this.type = "Map";
                btnWERT.IsVisible = false;
                btnMain.Text = "Send Proposal";
            try
            {
                Uri uri = new Uri(_context.ProfileImageLink);
                var ab = UriImageSource.FromUri(uri);
                ProfileImg.Source = ab;
            }
            catch (Exception ex)
            { 
            
            }
        }
        public ProfilePage(string id, string ProposalId)
        {
            Services.WebService web = new Services.WebService();
            var a = web.UserService.GetUser(id);
            _context = new ProfileViewModel(a);
            InitializeComponent();
            this.type = "Proposal";
            btnWERT.Text = "Reject";
            btnMain.Text = "Accept";
            this.BindingContext = _context;
            this.proposalId = ProposalId;
            _context.ProfileImageLink = _context.ProfileImageLink.Substring(1, _context.ProfileImageLink.Length - 2);
            Uri uri = new Uri(_context.ProfileImageLink);
            var ab = UriImageSource.FromUri(uri);
            ProfileImg.Source = ab;
        }
        private async void BtnWERT_Clicked(object sender, EventArgs e)
        {
            if (type == "Proposal")
            {
                Services.WebService web = new Services.WebService();
                await web.ConversationService.ApiConversationUpdateProposalResponsePutAsync(proposalId, false);

                await DisplayAlert("Proposal Declined", "You rejected the proposal", "Okay");
            }
            else
            Navigation.PushAsync(new Views.Profile.EditUserProfilePage());
        }

        private async void btnMain_Clicked(object sender, EventArgs e)
        {
            if (type == "Map")
            {
                Services.WebService web = new Services.WebService();
                var temp = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);
                await web.ConversationService.ApiConversationAddProposalPostAsync(temp.Id, _context.Id);
                await DisplayAlert("Proposal Sent", "Proposal has been sent", "Okay");
            }
            if (type == "Proposal")
            {
                Services.WebService web = new Services.WebService();
                await web.ConversationService.ApiConversationUpdateProposalResponsePutAsync(proposalId, true);
               
                await DisplayAlert("Proposal Accepted", "Proceed to chat room", "Okay");
            }
           
            else
                Application.Current.MainPage = new Views.Master.MasterMainPageDetail();
            //Navigation.PushAsync(new Views.Master.MasterMainPageDetail());

        }
    }
}