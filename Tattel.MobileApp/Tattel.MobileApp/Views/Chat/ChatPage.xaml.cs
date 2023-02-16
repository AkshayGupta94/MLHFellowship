using Tattel.MobileApp.Model;
using Tattel.MobileApp.Utilities;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Tattel.MobileApp.ViewModels.Master;
using Tattel.MobileApp.ViewModels.Chat;

namespace Tattel.MobileApp.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        ProposalDTO proposal;
        ChatModel mChat = new ChatModel();
        List<ChatModel> mChats = new List<ChatModel>();
        public ChatPageViewModel _context;
        public ChatPage()
        {
            try
            {
                InitializeComponent();
                mChats = new List<ChatModel>
                      {
                         new ChatModel(){Id = 1, Time = "10:32 AM", txtMessage = "Abc,Hellow Wrld.Java is cool", UserImage = "iconUserImage", DarkBackGround = true, OrangeBackGround = false, },
                         new ChatModel(){Id = 2, Time = "10:42 AM", txtMessage = "it is. But I am a .NET Developer", EndUserImage = "iconUserImage", DarkBackGround = false, OrangeBackGround = true, },
                         new ChatModel(){Id = 2, Time = "10:45 AM", txtMessage = ".NET is also really good OOP is Awesome.", EndUserImage = "iconUserImage", DarkBackGround = false, OrangeBackGround = true, },
                         new ChatModel(){Id = 1, Time = "10:53 AM", txtMessage = "Sure Tine!", UserImage = "iconUserImage", DarkBackGround = true, OrangeBackGround = false, },
                         new ChatModel(){Id = 1, Time = "11:12 AM", txtMessage = "Meet You today at 5pm. Starbucks!", UserImage = "iconUserImage", DarkBackGround = true, OrangeBackGround = false, },
                      };
                flvMain.FlowItemsSource = mChats.ToList();
            }
            catch (Exception ex)
            {
                var exp = ex.Message;
            }
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        public  void ShowWarning()
        {
            Dispatcher.BeginInvokeOnMainThread(() =>
            {
                DisplayAlert("5 minute warning", "The chat will end in 5 minutes", "Okay");

            });
        }
        public  void ShowEnd()
        {
            try
            {
                Dispatcher.BeginInvokeOnMainThread(() =>
                { 
                    DisplayAlert("Chat Ended", "The chat has now ended. You may not send new messages", "Okay");
                });
                
            }
            catch (Exception ex)
            { 
            
            }
        }
        public void ShowInvite(string id)
        {
            try
            {
                if (id == _context.EndUserId)
                {
                    Dispatcher.BeginInvokeOnMainThread(async () =>
                    {
                        var resp = await DisplayAlert("Invitation to meet", "The person has asked you to meet. Do you want to share your location ?", "Yes", "No");
                        if (resp == true)
                        {
                            Services.WebService webService = new Services.WebService();
                            await webService.ConversationService.ApiConversationAcceptMeetProposalPostAsync(_context.EndUserId, _context.UserId);
                            bool ans=await DisplayAlert("Advisory", "1) Please make sure you are meeting in a public space\n" +
                                "2) We take no responsibility in what happens in the meeting\n" +
                                "3) You agree that you are meeting the party at your own risk\n" +
                                "4) By continuing you agree that your are the age of 18 & above\n" +
                                "5) You confirm that you are not meeting the opposite party for any criminal offence or act or to cause harm to one another\n" +
                                "6) You agree to all the above","Yes","No");
                            //TODO: start navigation
                            if (ans)
                            {
                                var loc = await webService.UserService.GetUserAsync(_context.EndUserId);
                                var location = loc.Latitude.ToString() + "," + loc.Longitude.ToString();
                                if (Device.RuntimePlatform == Device.iOS)
                                {

                                    await Launcher.OpenAsync("http://maps.apple.com/?daddr=" + location);
                                }
                                else if (Device.RuntimePlatform == Device.Android)
                                {

                                    await Launcher.OpenAsync("http://maps.google.com/?daddr=" + location);
                                }
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void AcceptedInvite(string id)
        {
            try
            {
                if (id == _context.EndUserId)
                {
                    Dispatcher.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Invitation to meet accepted", "Starting Navigation", "Okay");
                        Services.WebService webService = new Services.WebService();
                    bool ans = await DisplayAlert("Advisory", "1) Please make sure you are meeting in a public space\n" +
                            "2) We take no responsibility in what happens in the meeting\n" +
                            "3) You agree that you are meeting the party at your own risk\n" +
                            "4) By continuing you agree that your are the age of 18 & above\n" +
                            "5) You confirm that you are not meeting the opposite party for any criminal offence or act or to cause harm to one another\n" +
                            "6) You agree to all the above", "Yes", "No");
                        //TODO: start navigation
                        if (ans)
                        {
                            //TODO: start navigation
                            var loc = await webService.UserService.GetUserAsync(_context.EndUserId);
                            var location = loc.Latitude.ToString() + "," + loc.Longitude.ToString();
                            if (Device.RuntimePlatform == Device.iOS)
                            {

                                await Launcher.OpenAsync("http://maps.apple.com/?daddr=" + location);
                            }
                            else if (Device.RuntimePlatform == Device.Android)
                            {

                                await Launcher.OpenAsync("http://maps.google.com/?daddr=" + location);
                            }
                        }

                    });
                }
            }
            catch (Exception ex)
            {

            }
        }
        bool flag = false;
        public ChatPage(ProposalDTO payload)
        {
            InitializeComponent();
            try
            {
                flag = false;
                proposal = payload;
                _context = new ChatPageViewModel(payload);
                ((App)(Xamarin.Forms.Application.Current)).chatPage = this;
                 ((App)(Xamarin.Forms.Application.Current)).PageOpen = "Chat";
                this.BindingContext = _context;
                flag = true;
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", ex.Message, "Okay");
            }
        }
        protected override void OnAppearing()
        {
            if (flag&&proposal!=null)
            {
                _context = new ChatPageViewModel(proposal);
                ((App)(Xamarin.Forms.Application.Current)).chatPage = this;
                ((App)(Xamarin.Forms.Application.Current)).PageOpen = "Chat";
                this.BindingContext = _context;
            }
            base.OnAppearing();
            mChat.currentDeviceName = DeviceInfo.Name;
        }

        private async void ImgChatNotification_Tapped(object sender, EventArgs e)
        {
            await imgChatNotification.ScaleTo(1.2, 200, Easing.SpringIn);
            await imgChatNotification.ScaleTo(1, 200, Easing.Linear);
            Services.WebService webService = new Services.WebService();
            await webService.ConversationService.ApiConversationSendMeetProposalPostAsync(_context.UserId,_context.EndUserId);

           // PopupNavigation.PushAsync(new Views.Chat.ChatNotification.ChatNotificationPopupPage(), false);
        }

        private void ExtEntry_Focused(object sender, FocusEventArgs e)
        {
            grdText.Focus();
            //if (Device.OS == TargetPlatform.Android)
            //{
            //    grdText.Padding = new Thickness(10, 0, 10, 288);
            //}

            //else

            //{
            //    if (mChat.currentDeviceName == mChat.iPhone_SE || mChat.currentDeviceName == mChat.iPhone_5s)
            //    {
            //        grdText.Padding = new Thickness(10, 0, 10, 250);
            //    }

            //    else if (mChat.currentDeviceName == mChat.iPhone_X || mChat.currentDeviceName == mChat.iPhone_XS)
            //    {
            //        grdText.Padding = new Thickness(10, 0, 10, 300);
            //    }

            //    else if (mChat.currentDeviceName == mChat.iPhone_XR || mChat.currentDeviceName == mChat.iPhone_XS_Max)
            //    {
            //        grdText.Padding = new Thickness(10, 0, 10, 310);
            //    }

            //    else if (mChat.currentDeviceName == mChat.iPhone_8 || mChat.currentDeviceName == mChat.iPhone_7 || mChat.currentDeviceName == mChat.iPhone_6s || mChat.currentDeviceName == mChat.iPhone_6)
            //    {
            //        grdText.Padding = new Thickness(10, 0, 10, 260);
            //    }

            //    else
            //    {
            //        grdText.Padding = new Thickness(10, 0, 10, 270);
            //    }
            //}
        }

        private void ExtEntry_Unfocused(object sender, FocusEventArgs e)
        {
            grdText.Unfocus();
            //grdText.Padding = new Thickness(10, 0, 10, 0);
        }



        private async void SendMessage_Tapped(object sender, EventArgs e)
        {
            Services.WebService webService = new Services.WebService();
            try
            {
                await webService.ConversationService.ApiConversationAddMessagePostAsync(new WebApiClient.Model.Message { ReceiverUserId = _context.EndUserId, SenderUserId = _context.UserId, 
                    TimeStamp = DateTime.Now,
                 MessageText=ChatMessage.Text},_context.ProposalId);
                _context.Chats.Add(new ChatDTO
                {
                    Id = 1,
                    txtMessage = ChatMessage.Text,
                    Time = DateTime.Now.ToShortTimeString(),
                    DarkBackGround = false,
                    OrangeBackGround = true,
                    UserImageSource = _context.UserImage
                });
                ChatMessage.Text = "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "okay");
            }
        }
        protected override void OnDisappearing()
        {
            ((App)(Xamarin.Forms.Application.Current)).chatPage = null;
            ((App)(Xamarin.Forms.Application.Current)).PageOpen = "";
            base.OnDisappearing();
        }
    }
}