using Foundation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using UserNotifications;

namespace Tattel.MobileApp.iOS
{
    class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
    {
        public UserNotificationCenterDelegate()
        {
        }
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            // Do something with the notification
            Console.WriteLine("Active Notification: {0}", notification);
            // Tell system to display the notification anyway or use
            // `None` to say we have handled the display locally.

            UNNotificationRequest request = notification.Request;
            UNNotificationContent content = request.Content;

            try
            {
                switch (content.UserInfo["notificationType"].ToString())
                {
                    case "create":
                        if (((Tattel.MobileApp.App)App.Current).PageOpen == "Map")
                        {
                            ((Tattel.MobileApp.App)App.Current).mapPage?.ShowNotification();
                            completionHandler(UNNotificationPresentationOptions.Alert);
                        }
                        else
                        {
                            completionHandler(UNNotificationPresentationOptions.Alert);
                        }
                        break;
                    case "accept":
                        completionHandler(UNNotificationPresentationOptions.Alert);
                        break;
                    case "alert":
                        if (((Tattel.MobileApp.App)App.Current).PageOpen == "Chat")
                        {
                            if (((Tattel.MobileApp.App)App.Current).chatPage._context.ProposalId == (content.UserInfo["extras"].ToString()))
                            {
                                ((Tattel.MobileApp.App)App.Current).chatPage.ShowWarning();
                            }
                            else
                            {
                                completionHandler(UNNotificationPresentationOptions.Alert);
                            }
                        }
                        else
                        {
                            completionHandler(UNNotificationPresentationOptions.Alert);
                        }
                        break;
                    case "over":
                        if (((Tattel.MobileApp.App)App.Current).PageOpen == "Chat")
                        {
                            if (((Tattel.MobileApp.App)App.Current).chatPage._context.ProposalId == (content.UserInfo["extras"].ToString()))
                            {
                                ((Tattel.MobileApp.App)App.Current).chatPage.ShowEnd();
                            }
                            else
                            {
                                completionHandler(UNNotificationPresentationOptions.Alert);
                            }
                        }
                        else
                        {
                            completionHandler(UNNotificationPresentationOptions.Alert);
                        }
                        break;
                    case "message":

                        if (((Tattel.MobileApp.App)App.Current).PageOpen == "Chat")
                        {

                            var mess = JsonConvert.DeserializeObject<WebApiClient.Model.Message>(content.UserInfo["extras"].ToString());
                            {
                                ((Tattel.MobileApp.App)App.Current).chatPage._context.AddMessage(mess);
                            }
                        }
                        else
                        {
                            completionHandler(UNNotificationPresentationOptions.Alert);
                        }
                        break;
                    case "meet-proposal":
                        if (((Tattel.MobileApp.App)App.Current).PageOpen == "Chat")
                        {
                            ((Tattel.MobileApp.App)App.Current).chatPage.ShowInvite((content.UserInfo["extras"].ToString()));

                        }
                        break;
                    case "meet-accept":
                        if (((Tattel.MobileApp.App)App.Current).PageOpen == "Chat")
                        {
                            ((Tattel.MobileApp.App)App.Current).chatPage.AcceptedInvite(content.UserInfo["extras"].ToString());

                        }
                        break;
                }
            }
            catch (Exception ex)
            { 
            
            }
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}