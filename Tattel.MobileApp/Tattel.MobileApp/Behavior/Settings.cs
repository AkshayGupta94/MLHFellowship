using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Tattel.MobileApp.Behavior
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string First = "first_key";
        private const string Phone = "Phone_key";
        private const string Profile = "Profile_key";

        private static readonly string SettingsDefault = "yes";
        private static readonly string PhoneNumberVerified = "no";
        private static readonly string UserProfileUploaded = "no";


        #endregion
        public static string FirstTime
        {
            get
            {
                return AppSettings.GetValueOrDefault(First, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(First, value);
            }
        }

        public static string PageName
        {
            get
            {
                return AppSettings.GetValueOrDefault("online", SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue("online", value);
            }
        }


        public static string NumberVerifiedSetting
        {
            get
            {
                return AppSettings.GetValueOrDefault(Phone, PhoneNumberVerified);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Phone, value);
            }
        }
        public static string UserProfileCreated
        {
            get
            {
                return AppSettings.GetValueOrDefault(Profile, UserProfileUploaded);
            }
            set
            {
                AppSettings.AddOrUpdateValue(Profile, value);
            }
        }
    }
}
