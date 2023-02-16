using System;
namespace Tattel.WebApiClient.Models
{
    public class ClientConnection
    {
        public ClientConnection()
        {
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime TimeConnected { get; set; }
        public DateTime TimeDisconnected { get; set; }
    }
}
