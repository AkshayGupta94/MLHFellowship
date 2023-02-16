using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Tattel.MobileApp.Services
{
    public class CachingService
    {
        public string GetCache(string ClassName)
        {
            if (Application.Current.Properties.ContainsKey(ClassName))
            {
                return Application.Current.Properties[ClassName].ToString();
            }
            else
                return "";
        }
        public async void PutCache(string ClassName,string Data)
        {
            if (Application.Current.Properties.ContainsKey(ClassName))
            {
                Application.Current.Properties[ClassName] = Data;
            }
            else
            {
                Application.Current.Properties.Add(ClassName, Data);

            }
            await Application.Current.SavePropertiesAsync();
        }
    }
}
 