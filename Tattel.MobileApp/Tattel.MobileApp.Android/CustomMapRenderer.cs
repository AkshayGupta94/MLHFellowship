using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Net;
using System.Text;
using Tattel.MobileApp.CustomUI;
using Tattel.MobileApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace Tattel.MobileApp.Droid
{
    public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
    {
        List<CustomPin> customPins;

        public CustomMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.InfoWindowClick -= OnInfoWindowClick;
            }

            if (e.NewElement != null)
            {
                var formsMap = (CustomMap)e.NewElement;
                customPins = formsMap.CustomPins;
            }
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.InfoWindowClick += OnInfoWindowClick;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            if (pin.Label == "Me")
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.Pinme));
            else
                marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
            // marker.SetSnippet(pin.Address);

            return marker;
        }

        void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }

          
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

               
                    view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
                

                var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
                //var infoImage = view.FindViewById<ImageView>(Resource.Id.InfoWindowImage);
                var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
                var infoSubtitle2 = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle2);

                if (infoTitle != null)
                {
                    infoTitle.Text = customPin.Sector;
                }
                if(infoSubtitle!= null)
                {
                    infoSubtitle.Text = customPin.Role;
                }
                if (infoSubtitle2 != null)
                {
                    infoSubtitle2.Text = customPin.Organisation;
                }
                //if(infoImage!=null)
                //{
                //    var imageBitmap = GetImageBitmapFromUrl(customPin.ImageUrl);
                //    infoImage.SetImageBitmap(imageBitmap);

                //}
                
                
                return view;
            }
            return null;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }
            return null;
        }
        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;
            try
            {
                using (var webClient = new WebClient())
                {
                    url = url.Replace(" ", "%20");
                    var imageBytes = webClient.DownloadData(url);
                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            { 
            }
            return imageBitmap;
        }

    }
}