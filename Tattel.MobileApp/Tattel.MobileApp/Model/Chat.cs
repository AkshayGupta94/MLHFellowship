using Tattel.MobileApp.Utilities;
using Tattel.MobileApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tattel.MobileApp.Model
{
   public class ChatModel : IChat
    {
        public int Id { get; set; }
        public string txtMessage { get; set; }
        public string UserImage { get; set; }
        public string EndUserImage { get; set; }
        public string Time { get; set; }
        public bool DarkBackGround { get; set; }
        public bool OrangeBackGround { get; set; }

       


        public string currentDeviceName;
        public string iPhone_X = DeviceName.iPhone_X.ToString().Replace("_", " ");
        public string iPhone_SE = DeviceName.iPhone_SE.ToString().Replace("_", " ");
        public string iPhone_XS = DeviceName.iPhone_XS.ToString().Replace("_", " ");
        public string iPhone_XR = DeviceName.iPhone_XR.ToString().Replace("_", " ");
        public string iPhone_XS_Max = DeviceName.iPhone_XS_Max.ToString().Replace("_", " ");
        public string iPhone_8 = DeviceName.iPhone_8.ToString().Replace("_", " ");
        public string iPhone_7 = DeviceName.iPhone_7.ToString().Replace("_", " ");
        public string iPhone_6s = DeviceName.iPhone_6s.ToString().Replace("_", " ");
        public string iPhone_6 = DeviceName.iPhone_6.ToString().Replace("_", " ");
        public string iPhone_5s = DeviceName.iPhone_5s.ToString().Replace("_", " ");
    }
}
