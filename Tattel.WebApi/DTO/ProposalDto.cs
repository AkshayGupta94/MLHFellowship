using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tattel.WebApi.DataModels;

namespace Tattel.WebApi.DTO
{
    public class ProposalDto
    {
        public string Id { get; set; }
        public string SendUserId { get; set; }
        public string RecipientUserId { get; set; }
        public DateTime InitiatedAt { get; set; } //DateTime variable tracking when the conversation was initiated
        public bool? IsAccepted { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SenderName { get; set; }
        public string SenderProfilePic { get; set; }
        public string SenderInterests { get; set; }
        public string RecipientProfilePic { get; set; }
        public string RecipientName { get; set; }
    }
}
