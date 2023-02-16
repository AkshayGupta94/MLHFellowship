using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.Model;
using Tattel.MobileApp.ViewModels;
using Tattel.MobileApp.ViewModels.Chat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Chat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
		bool isonline;
		MapPageViewModel context;
		public MapPage(MapFilterQuery query)
		{
			isonline = true;
			context = new MapPageViewModel(this.Navigation);
			context.mapFilterQuery = query;
			InitializeComponent();
			this.BindingContext = context;
			context.PinTapped += Context_PinTapped;
			((App)(Xamarin.Forms.Application.Current)).PageOpen = "Map";
			((App)(Xamarin.Forms.Application.Current)).mapPage = this;

			LoadPage();

		}
		public MapPage()
		{
			isonline = true;
			context = new MapPageViewModel(this.Navigation);
			InitializeComponent();
			this.BindingContext = context;
			context.PinTapped += Context_PinTapped;
			((App)(Xamarin.Forms.Application.Current)).PageOpen = "Map";
			((App)(Xamarin.Forms.Application.Current)).mapPage = this;

			LoadPage();

		}
		public void ShowNotification()
		{
			Dispatcher.BeginInvokeOnMainThread(async() =>
			{
				await DisplayAlert("New Proposal", "You recieved a new proposal. Proceed to proposal page to view it", "Okay");
			});
			
		}
        private async void Context_PinTapped(object sender, EventArgs e)
        {
			bool a =await DisplayAlert("Send proposal", "Do you want to send this user a proposal", "Yes", "No");
			if (a)
			{
				try
				{
					Services.WebService web = new Services.WebService();
					var temp = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);
					await web.ConversationService.ApiConversationAddProposalPostAsync(temp.Id, context.SenderId);
					await DisplayAlert("Proposal Sent", "Proposal has been sent", "Okay");
				}
				catch (Exception ex)
				{
					await DisplayAlert("Error", ex.Message, "Okay");
				}
			}
		}

        protected override async void OnAppearing()
        {
			Services.WebService webService = new Services.WebService();
			var _context = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);
			isonline = false;
			((App)(Xamarin.Forms.Application.Current)).PageOpen = "";
			((App)(Xamarin.Forms.Application.Current)).mapPage = null;
			try
			{
				await webService.UserService.ApiUserUpdateStatusUpdateStatusPutAsync(_context.Id, true);
			}
			catch (Exception ex)
			{

			}
			isonline = true;
			Device.StartTimer(new TimeSpan(0, 5, 0), () =>
			{
				if (isonline)
				{
					context.SetMap();
				}
				return isonline;
			});

			base.OnAppearing();
        }
        private async void LoadPage()
		{ 
			await context.LoadData();
			
			Device.StartTimer(new TimeSpan(0, 5, 0), () =>
			   {
				   if (isonline)
				   {
					   context.SetMap();
				   }
				   return isonline;
			   }
			);
			if (Device.RuntimePlatform == Device.iOS)
			{
				
				MessagingCenter.Subscribe<App, string>(this, "Hi", async (sender2, args) => {
					bool a = await DisplayAlert("Send proposal", "Do you want to send this user a proposal", "Yes", "No");
					if (a)
					{
						try
						{
							Services.WebService web = new Services.WebService();
							var temp = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);
							await web.ConversationService.ApiConversationAddProposalPostAsync(temp.Id, args);
							await DisplayAlert("Proposal Sent", "Proposal has been sent", "Okay");
						}
						catch (Exception ex)
						{
							await DisplayAlert("Error", ex.Message, "Okay");
						}
					}
				});

			}

		}

        protected override async void OnDisappearing()
        {
			Services.WebService webService = new Services.WebService();
			var _context = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);
			isonline = false;
			((App)(Xamarin.Forms.Application.Current)).PageOpen = "";
			((App)(Xamarin.Forms.Application.Current)).mapPage = null;
			try
			{
				await webService.UserService.ApiUserUpdateStatusUpdateStatusPutAsync(_context.Id, false);
			}
			catch (Exception ex)
			{
				
			}
			base.OnDisappearing();
        }
        private void ImgLacation_Tapped(object sender, EventArgs e)
        {
           //avigation.PushAsync(new Views.Chat.ChatPage());
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
			Navigation.PushAsync(new MapPageFilter());
        }

        //     private void picker_SelectedIndexChanged(object sender, EventArgs e)
        //     {
        //switch (picker.SelectedIndex)
        //         {
        //	case 0:
        //		context.MinAge = 18;context.MaxAge = 25;
        //		break;
        //	case 1:
        //		context.MinAge = 26; context.MaxAge = 35;

        //		break;
        //	case 2:
        //		context.MinAge = 36; context.MaxAge = 50;

        //		break;
        //	case 3:
        //		context.MinAge = 51; context.MaxAge = 65;

        //		break;
        //	case 4:
        //		context.MinAge = 66; context.MaxAge = 100;

        //		break;

        //         }
        //context.SetMap();
        //     }
    }
}