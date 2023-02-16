using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tattel.MobileApp.Droid
{
    
        public class Handle
        {
            public string emailAddress { get; set; }
        }

        public class Element
        {
            [JsonProperty("handle~")]
            public Handle Handle { get; set; }
            public string handle { get; set; }
        }

        public class LinkedInResponse
        {
            public List<Element> elements { get; set; }
        }
   
    
}