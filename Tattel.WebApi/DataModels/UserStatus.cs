using System;
namespace Tattel.WebApi.DataModels
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
