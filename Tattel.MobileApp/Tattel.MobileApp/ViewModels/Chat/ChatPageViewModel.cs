using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Tattel.MobileApp.Model;
using Tattel.MobileApp.ViewModels.Master;
using Tattel.WebApiClient.Model;
using Xamarin.Forms;

namespace Tattel.MobileApp.ViewModels.Chat
{
    public class ChatPageViewModel:BaseViewModel
    {
        public ObservableCollection<ChatDTO> _Chats;

        public ObservableCollection<ChatDTO> Chats 
        {
            get
            {
                return _Chats;
            }
            set
            {
                _Chats = value;
                OnPropertyChanged();
            }
        }
        public ImageSource UserImage { get; set; }
        public ImageSource EndUserImage { get; set; }
        public string EndUserName { get; set; }
        public string EndUserId { get; set; }
        public string UserId { get; set; }
       
        public string ProposalId { get; set; }
     
        public ChatPageViewModel(ProposalDTO proposal)
        {
            var a = proposal.ProfilePicPath;
            this.EndUserImage = UriImageSource.FromUri(new Uri(proposal.ProfilePicPath));
            this.EndUserId = proposal.SenderId;
         
            this.UserId = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]).Id;
            this.EndUserName = proposal.Name;
            this.UserImage = UriImageSource.FromUri(new Uri(JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]).ProfileImageLink));
            this.ProposalId = proposal.Id;
            LoadChats();
        }

        public async void LoadChats()
        {
            try
            {
                Chats = new ObservableCollection<ChatDTO>();
                Services.WebService webService = new Services.WebService();
                var messages = await webService.ConversationService.ApiConversationGetMessagesGetAsync(this.ProposalId, 1);
               

                foreach (Message m in messages)
                {
                    ChatDTO chatModel = new ChatDTO { Id = 1, txtMessage = m.MessageText, Time = ((DateTime)m.TimeStamp).ToShortTimeString() };
                    
                    if (m.SenderUserId == this.EndUserId)
                    {
                        chatModel.DarkBackGround = true;
                        chatModel.OrangeBackGround = false;
                        chatModel.UserImageSource = this.EndUserImage;
                    }
                    else
                    {

                        chatModel.DarkBackGround = false;
                        chatModel.OrangeBackGround = true;
                        chatModel.UserImageSource = this.UserImage;

                    }
                    Chats.Add(chatModel);
                   
                }
              

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddMessage(Message m)
        { 
            ChatDTO chatModel = new ChatDTO { Id = 1, txtMessage = m.MessageText, Time = ((DateTime)m.TimeStamp).ToShortTimeString() };
            if (m.SenderUserId == this.EndUserId)
            {
                chatModel.DarkBackGround = true;
                chatModel.OrangeBackGround = false;
                chatModel.UserImageSource = this.EndUserImage;
            }
            else
            {

                chatModel.DarkBackGround = false;
                chatModel.OrangeBackGround = true;
                chatModel.UserImageSource = this.UserImage;

            }
            Chats.Add(chatModel);
        }

    }

    public class ChatDTO : ChatModel
    { 
    
      public ImageSource UserImageSource { get; set; }

    }
}

