using System;
namespace Tattel.WebApiClient.Models
{
    public class UserStatus
    {
        public UserStatus()
        {
        }

        public string UserId { get; set; }
        public enum State
        {
            Available,
            Busy
        }
    }
}
