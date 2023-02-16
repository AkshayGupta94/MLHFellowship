using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace Tattel.MobileApp.CustomUI
{
    public class CustomPin:Pin 
    {
        public string ImageUrl { get; set; }
        public string Id { get; set; }
        public string Role { get; set; }
        public string Organisation { get; set; }
        public string Sector { get; set; }

    }
}
