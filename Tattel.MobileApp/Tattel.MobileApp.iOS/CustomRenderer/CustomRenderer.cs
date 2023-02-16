using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tattel.MobileApp.Extension;
using Tattel.MobileApp.iOS.CustomRenderer;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtEntry), typeof(ExtEntryCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtPicker), typeof(FontFamilyCustomRendererPickerExt))]
namespace Tattel.MobileApp.iOS.CustomRenderer
{

    public class ExtEntryCustomRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (Control != null)
                {
                    Control.BorderStyle = UITextBorderStyle.RoundedRect;

                    Control.Layer.BorderWidth = 0;
                    Control.Layer.BackgroundColor = Color.Transparent.ToCGColor();
                    Control.Layer.CornerRadius = (nfloat)0.0;
                    Control.LeftView = new UIView(new CGRect(0, 0, 5, 0));
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                    Control.RightView = new UIView(new CGRect(0, 0, 5, 0));
                    Control.RightViewMode = UITextFieldViewMode.Always;
                    this.Control.BorderStyle = UITextBorderStyle.None;

                }
            }
            catch (Exception ex)
            {
                var errorr = ex.Message;
            }
        }
    }

    public class FontFamilyCustomRendererPickerExt : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            try
            {
                if (Control != null)
                {
                    Control.Layer.BackgroundColor = Color.Transparent.ToCGColor();
                    Control.Layer.CornerRadius = (nfloat)0.0;
                    Control.LeftView = new UIView(new CGRect(0, 0, 5, 0));
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                    Control.RightView = new UIView(new CGRect(0, 0, 5, 0));
                    Control.RightViewMode = UITextFieldViewMode.Always;
                    this.Control.BorderStyle = UITextBorderStyle.None;
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
        }
    }

}