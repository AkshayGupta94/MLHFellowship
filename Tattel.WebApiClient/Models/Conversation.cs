using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tattel.WebApiClient.Models
{
    public class Conversation
    {
        public string Id { get; set; }
        public string SenderUserId { get; set; }
        public string RecipientUserId { get; set; }
        public DateTime InitiatedAt { get; set; } //DateTime variable tracking when the conversation was initiated
        public bool IsAccepted { get; set; }
        public List<Message> Messages { get; set; }
        public enum EndState
        {
            AcceptMeet,
            DeclineMeet
        } //Enum tracking if a conversation ended with or without a meet
        public string MeetInitiatedUserId { get; set; } //User Id of the person who initiated the meet
        public DateTime MeetInitiatedAtTimeStamp { get; set; } //Timestamp of when the Meet was initiated
        public enum MeetState
        {
            AcceptMeet,
            DeclineMeet
        }
    }
}