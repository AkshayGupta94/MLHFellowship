using System;
namespace Tattel.WebApiClient.Models
{
    public class Message
    {
        public Message()
        {
        }

        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string MessageText { get; set; }
    }
}

