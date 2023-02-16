using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Tattel.MobileApp.Extension
{
    public class RoundedCornerView : Grid
    {
        public static readonly BindableProperty RoundedCornerRadiusProperty = BindableProperty.Create<RoundedCornerView, double>(w => w.RoundedCornerRadius, 3);
        public double RoundedCornerRadius
        {
            get
            {
                return (double)GetValue(RoundedCornerRadiusProperty);
            }
            set
            {
                SetValue(RoundedCornerRadiusProperty, value);
            }
        }
    }
}
