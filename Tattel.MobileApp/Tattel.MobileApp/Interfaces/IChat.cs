using System;
using System.Collections.Generic;
using System.Text;

namespace Tattel.MobileApp.Interfaces
{
    public interface IChat
    {
        public int Id { get; }
        public string txtMessage { get; }
        public string UserImage { get; }
        public string EndUserImage { get; }
        public string Time { get; }
        public bool DarkBackGround { get; }
        public bool OrangeBackGround { get; }
    }
}
