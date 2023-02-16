using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson.Serialization.Attributes;

namespace Tattel.WebApi.DataModels
{
    public class Conversation
    {
        [BsonId]
        public string Id { get; set; }
        public string SendUserId { get; set; }
        public string RecipientUserId { get; set; }
        public DateTime InitiatedAt { get; set; } //DateTime variable tracking when the conversation was initiated
        public bool? IsAccepted { get; set; }
        public List<Message> Messages { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
