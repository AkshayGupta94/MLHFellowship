using System;
using System.IO;

using Microsoft.Extensions.Configuration;

using Tattel.MobileApp.Interfaces;

using Xamarin.Essentials;

namespace Tattel.MobileApp.Configuration
{
    public class ConfigurationClass : IConfigurationClass
    {
        public string messageBirdApiKey { get; set; }

        public ConfigurationClass()
        {
            messageBirdApiKey = "FBbLhAhqrWI2482pfvVf2Xdqg";
        }
    }
}
