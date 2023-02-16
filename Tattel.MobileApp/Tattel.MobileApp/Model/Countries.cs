using System;
using System.Collections.Generic;
using System.Text;

namespace Tattel.MobileApp.Model
{
    public class Countries
    {
        private string _name { get; set; }
        public string name {

            get
            {
                return _name;
            }
            set
            {
                
                _name = value;
                Text = _name;
            }
        }
            public string dial_code { get; set; }
        private string _code;
            public string code 
        {

            get
            {
                return _code;
            }
            set
            {
                _code = value;
                ImageSource = "https://www.countryflags.io/" + _code + "/flat/64.png";
            } 
        }
        public string ImageSource { get; set; }
        public string Text { get; set; }


    }
}
