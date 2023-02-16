using Foundation;
using MapKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace Tattel.MobileApp.iOS
{
    public class CustomMKAnnotationView : MKAnnotationView
    {
		public string Name { get; set; }

		public string Id { get; set; }

		public string Organisation { get; set; }

		public string Role { get; set; }

		public CustomMKAnnotationView(IMKAnnotation annotation, string id)
			: base(annotation, id)
		{
		}
	}
}