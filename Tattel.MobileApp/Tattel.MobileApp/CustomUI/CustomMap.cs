using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace Tattel.MobileApp.CustomUI
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
        public CustomMap(MapSpan mapSpan):base(mapSpan)
        {
           
        }
    }
}
