using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tattel.MobileApp.ViewModels.Master
{
    public class ProposalViewModel:BaseViewModel
    {
        ProfileViewModel _context;
        private ObservableCollection<ProposalDTO> _items;
        public ObservableCollection<ProposalDTO> Items 
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }
        public ProposalViewModel()
        {
            _context = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);
            Items = new ObservableCollection<ProposalDTO>();
        }
        public async Task LoadData()
        {
            IsBusy = true;
            try
            {
                LoadingStatus = "Getting Proposals";
                Services.WebService web = new Services.WebService();

                var a = await web.ConversationService.ApiConversationGetProposalsGetAsync(_context.Id);
                foreach (var prop in a)
                {
                    if (prop.IsAccepted == null)
                    {
                        if (prop.SendUserId != _context.Id)
                        {
                            Items.Add(new ProposalDTO { Interests = prop.SenderInterests, Id = prop.Id, Date = ((DateTime)prop.InitiatedAt).ToShortDateString(), Name = prop.SenderName, ProfilePicPath = prop.SenderProfilePic });
                        }
                    }

                }
                Items = Items;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                throw ex;
            }
            IsBusy = false; 
        }
        public async Task LoadChatData()
        {
            IsBusy = true;
            LoadingStatus = "Loading Chats";
            Services.WebService web = new Services.WebService();
            try
            {
                var a = await web.ConversationService.ApiConversationGetProposalsGetAsync(_context.Id);
                foreach (var prop in a)
                {
                    if (prop.IsAccepted == true)
                    {
                        if (prop.SendUserId != _context.Id)
                        {
                            Items.Add(new ProposalDTO { Id = prop.Id, SenderId = prop.SendUserId, Date = ((DateTime)prop.InitiatedAt).ToShortDateString(), Name = prop.SenderName, ProfilePicPath = prop.SenderProfilePic });
                        }
                        else 
                        {
                            Items.Add(new ProposalDTO { Id = prop.Id, SenderId = prop.RecipientUserId, Date = ((DateTime)prop.InitiatedAt).ToShortDateString(), Name = prop.RecipientName, ProfilePicPath = prop.RecipientProfilePic });

                        }
                    }

                }
                Items = Items;
                IsBusy = false;
            }
            catch (Exception ex)
            {
                IsBusy = false;
                throw ex;
            }

        }
    }
    public class ProposalDTO
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string ProfilePicPath { get; set; }
        public string Interests { get; set; }

        public string SenderId { get; set; }
        public string Id { get; set; }


    }
}
