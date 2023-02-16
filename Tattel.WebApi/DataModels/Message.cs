using System;
namespace Tattel.WebApi.DataModels
{
    public class Message
    {
        public Message()
        {
        }

        public string SenderUserId { get; set; }
        public string ReceiverUserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string MessageText { get; set; }
    }
}
