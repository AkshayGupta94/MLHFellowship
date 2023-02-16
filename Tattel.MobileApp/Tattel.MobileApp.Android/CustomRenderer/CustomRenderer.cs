using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Tattel.MobileApp.Droid.CustomRenderer;
using Tattel.MobileApp.Extension;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Xamarin.Forms.View;

[assembly: ExportRenderer(typeof(ExtEntry), typeof(ExtEntryCustomRenderer))]
[assembly: ExportRenderer(typeof(Label), typeof(LabelCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtButton), typeof(ExtButtonCustomRenderer))]
[assembly: ExportRenderer(typeof(ExtPicker), typeof(FontFamilyCustomRendererPickerExt))]
[assembly: ExportRenderer(typeof(RoundedCornerView), typeof(RoundedCornerViewRenderer))]
namespace Tattel.MobileApp.Droid.CustomRenderer
{
#pragma warning disable CS0618 // Type or member is obsolete

    public class ExtEntryCustomRenderer : EntryRenderer
    {
        Typeface font = null;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var label = (TextView)Control; // for example
                font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");

                label.Typeface = font;
            }

            var nativeedittextfield = (Android.Widget.EditText)this.Control;
            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(0);
            // gd.SetStroke(0, Android.Graphics.Color.ParseColor("#66000000"));
            gd.SetColor(Android.Graphics.Color.Transparent);
            nativeedittextfield.Background = gd;

            Control.SetPadding(0, 5, 5, 5);
            Control.Gravity = Android.Views.GravityFlags.CenterVertical;

        }

        //protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{

        //    base.OnElementPropertyChanged(sender, e);
        //    //if (e.PropertyName == "IsPassword")
        //    //{
        //    var fontFamilyName = ((ExtEntry)sender).FontFamily;
        //    Control.Typeface = Typeface.CreateFromAsset(Context.Assets, fontFamilyName);
        //    //}
        //}
    }

    public class LabelCustomRenderer : LabelRenderer
    {
        Typeface font = null;

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var label = (TextView)Control; // for example
                font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf"); 
                
                label.Typeface = font;
            }
        }
    }

    public class ExtButtonCustomRenderer : ButtonRenderer
    {
        Typeface font = null;

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            string fontFamily = e.NewElement?.FontFamily;

            Control.SetAllCaps(false);
            if (!string.IsNullOrEmpty(fontFamily))
            {
                var label = (TextView)Control; // for example
                font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");

                label.Typeface = font;
            }
        }
    }

    public class FontFamilyCustomRendererPickerExt : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            try
            {
                base.OnElementChanged(e);
                if (e.OldElement != null)
                {
                    return;
                }
                string fontFamily = e.NewElement?.FontFamily;
                if (!string.IsNullOrEmpty(fontFamily))
                {
                    var label = (TextView)Control; // for example
                    Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, fontFamily + ".otf");
                    label.Typeface = font;
                }

                // this.Control.Background = Forms.Context.GetDrawable(Resource.Drawable.CustomPickerBackground);
                //   Control?.SetPadding(20, 20, 20, 20);
                if (e.OldElement != null || e.NewElement != null)
                {
                    var customPicker = e.NewElement as ExtPicker;
                    // Control.TextSize *= (customPicker.FontSize * 0.01f);
                    // Control.SetHintTextColor(Android.Graphics.Color.ParseColor(customPicker.PlaceholderColor));
                    //customPicker.PlaceholderColor = Color.;
                    //Control.SetHintTextColor(Android.Graphics.Color.ParseColor(customPicker.PlaceholderColor));
                    //customPicker.ShowPicker = () => Control.PerformClick();
                }
                var nativeEditTextField = (Android.Widget.EditText)this.Control;
                GradientDrawable gd = new GradientDrawable();
                gd.SetCornerRadius(0);
                gd.SetColor(Android.Graphics.Color.Transparent);
                nativeEditTextField.Background = gd;
                Control.SetPadding(0, 5, 5, 5);
                Control.Gravity = Android.Views.GravityFlags.CenterVertical;
            }
            catch (System.Exception ex)
            {

            }
        }
    }

    public class RoundedCornerViewRenderer1 : ViewRenderer
    {
        //protected override void OnElementChanged(ElementChangedEventArgs<View> e) {  
        //    base.OnElementChanged(e);  
        //}  

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);
        }

        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            if (Element == null) return false;
            RoundedCornerView rcv = (RoundedCornerView)Element;
            this.SetClipChildren(true);
            rcv.Padding = new Thickness(0, 0, 0, 0);
            //rcv.HasShadow = false;      
            int radius = (int)(rcv.RoundedCornerRadius);
            // Check if make circle is set to true. If so, then we just use the width and      
            // height of the control to calculate the radius. RoundedCornerRadius will be ignored      
            // in this case.      

            //if (rcv.MakeCircle) {  
            //    radius = Math.Min(Width, Height) / 2;  
            //}  
            // When we create a round rect, we will have to double the radius since it is not      
            // the same as creating a circle.      
            radius *= 2;
            try
            {
                //Create path to clip the child       
                var path = new Path();
                path.AddRoundRect(new RectF(0, 0, Width, Height), new float[] {
                     radius,
                    radius,
                    radius,
                    radius,
                    0,
                    0,
                    0,
                    0
                }, Path.Direction.Ccw);
                canvas.Save();
                canvas.ClipPath(path);
                // Draw the child first so that the border shows up above it.      
                var result = base.DrawChild(canvas, child, drawingTime);
                canvas.Restore();
                /*   
                 * If a border is specified, we use the same path created above to stroke   
                 * with the border color.   
                 * */

                //if (rcv.BorderWidth > 0) {  
                //    // Draw a filled circle.      
                //    var paint = new Paint();  
                //    paint.AntiAlias = true;  
                //    paint.StrokeWidth = rcv.BorderWidth;  
                //    paint.SetStyle(Paint.Style.Stroke);  
                //    paint.Color = rcv.BorderColor.ToAndroid();  
                //    canvas.DrawPath(path, paint);  
                //    paint.Dispose();  
                //}  

                //Properly dispose      
                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.Message);
            }
            return base.DrawChild(canvas, child, drawingTime);
        }
    }

    public class RoundedCornerViewRenderer : ViewRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
        }

        protected override bool DrawChild(Canvas canvas, global::Android.Views.View child, long drawingTime)
        {
            if (Element == null) return false;

            RoundedCornerView rcv = (RoundedCornerView)Element;
            this.SetClipChildren(true);

            rcv.Padding = new Thickness(0, 0, 0, 0);
            //rcv.HasShadow = false;   

            int radius = (int)(rcv.RoundedCornerRadius);
            // Check if make circle is set to true. If so, then we just use the width and   
            // height of the control to calculate the radius. RoundedCornerRadius will be ignored   
            // in this case.   
            //if (rcv.MakeCircle)
            //{
            //    radius = Math.Min(Width, Height) / 2;
            //}

            // When we create a round rect, we will have to double the radius since it is not   
            // the same as creating a circle.   
            radius *= 2;

            try
            {
                //Create path to clip the child    
                var path = new Path();
                path.AddRoundRect(new RectF(0, 0, Width, Height),
                              new float[] { radius, radius, radius, radius, radius, radius, radius, radius },
                              Path.Direction.Ccw);

                canvas.Save();
                canvas.ClipPath(path);

                // Draw the child first so that the border shows up above it.   
                var result = base.DrawChild(canvas, child, drawingTime);

                canvas.Restore();

                /*  
                 * If a border is specified, we use the same path created above to stroke  
                 * with the border color.  
                 * */
                //if (rcv.BorderWidth > 0)
                //{
                //    // Draw a filled circle.   
                //    var paint = new Paint();
                //    paint.AntiAlias = true;
                //    paint.StrokeWidth = rcv.BorderWidth;
                //    paint.SetStyle(Paint.Style.Stroke);
                //    paint.Color = rcv.BorderColor.ToAndroid();

                //    canvas.DrawPath(path, paint);

                //    paint.Dispose();
                //}

                //Properly dispose   
                path.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                System.Console.Write(ex.Message);
            }

            return base.DrawChild(canvas, child, drawingTime);
        }
    }

#pragma warning restore CS0618 // Type or member is obsolete

}