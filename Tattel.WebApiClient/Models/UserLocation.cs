using System;
namespace Tattel.WebApiClient.Models
{
    public class UserLocation
    {
        public UserLocation()
        {
        }

        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public GPSLocation GPSLocation { get; set; }
    }
}
