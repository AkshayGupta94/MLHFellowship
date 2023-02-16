using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.Behavior;
using Tattel.MobileApp.Services;
using Tattel.MobileApp.ViewModels;
using Tattel.WebApiClient;
using Tattel.WebApiClient.Model;
using Tattel.WebApiClient.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Profile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditUserProfilePage : ContentPage, INotifyPropertyChanged
	{
        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<string> _ServerInterests;
        public ObservableCollection<string> ServerInterests 
        {
            get
            {
                return _ServerInterests;
            }
            set
            {
                _ServerInterests = value;

            }
        }
        ProfileViewModel _context;
        CachingService _cachingService;
        public EditUserProfilePage ()
		{
            Settings.PageName = "EditProfile";
            InitializeComponent();
            _cachingService = new CachingService();
            _context = JsonConvert.DeserializeObject<ProfileViewModel>(_cachingService.GetCache("User"));

            if (_context.Type == 2)
            {
                loadPage();
            }
            else
            {
                loadStudentPage();
                //loadInterests();
            }
            

        }
        private async void loadStudentPage()
        {
            try
            {
                ServerInterests = new ObservableCollection<string>();
                lblName6.BindingContext = ServerInterests;

                /// <remarks>setting page for student</remarks>
                EditIcon.IsVisible = true;
                lblName1.IsEnabled = true;
                lblAge.Text = "Enter the college/University name :";
                lblName3.Text = "Aspring Job role";
                lblName7.Text = "Aspiring Sector (please choose 1) ";


                if (check(_context.Name))
                {
                    lblName1.Text = _context.Name?.ToString();
                }
                if (check(_context.Designation))
                {
                    lblName5.Text = _context.Designation?.ToString();
                }
                if (check(_context.Organisation))
                {
                    Organisation.Text = _context.Organisation;
                }



                if (_context.Bio != null)
                {
                    Picker_Gender.SelectedItem = _context.Bio;
                }
                if (_context.DateOfBirth != null)
                {
                    DatePickerDOB.Date = _context.DateOfBirth;
                }
                lblName2.Text = string.IsNullOrWhiteSpace(_context.Name) ? "25" : (25 - _context.Name.Length).ToString();
                lblName4.Text = string.IsNullOrWhiteSpace(_context.Bio) ? "150" : (150 - _context.Bio.Length).ToString();


                Loading.IsRunning = true;
                Services.WebService webService = new WebService();
                var li = await webService.UserService.GetUserIntrestsAsync();
                foreach (var item in li)
                {
                    ServerInterests.Add(item);
                }
                lblName6.ItemsSource = ServerInterests;
                if (check(_context.Sector))
                {
                    lblName6.SelectedItem = _context.Sector;
                }
               
                Loading.IsRunning = false;

                if (check(_context.ProfileImageLink))
                {
                    Uri uri = new Uri(_context.ProfileImageLink);
                    var a = UriImageSource.FromUri(uri);

                    ProfilePic.Source = a;
                }
            }
            catch (Exception ex)
            { 
            
            }
        }
        private async void loadInterests()
        {
            ServerInterests = new ObservableCollection<string>();
            lblName6.BindingContext = ServerInterests;
            Loading.IsRunning = true;
            Services.WebService webService = new WebService();
            var li = await webService.UserService.GetUserIntrestsAsync();
            foreach (var item in li)
            {
                ServerInterests.Add(item);

            }
            lblName6.ItemsSource = ServerInterests;
         
            Uri uri = new Uri(_context.ProfileImageLink);
            var a = UriImageSource.FromUri(uri);
            lblName1.Text = _context.Name;
            ProfilePic.Source = a;
            lblName5.Text = _context.EmailAddress;
            Loading.IsRunning = false;
        }
        private async void loadPage()
        {
            ServerInterests = new ObservableCollection<string>();
            lblName6.BindingContext = ServerInterests;
            EditIcon.IsVisible = true;
            lblName1.IsEnabled = true;


            if (check(_context.Name))
            {
                lblName1.Text = _context.Name?.ToString();
            }
            if (check(_context.Designation))
            {
                lblName5.Text = _context.Designation?.ToString();
            }
            if (check(_context.Organisation))
            {
                Organisation.Text = _context.Organisation;
            }





            //await DisplayAlert("Alert", "Kindly select your interests again", "Okay");
            //lblName1.Text = _context.Name;
            //lblName5.Text= _context.Designation;
            //Organisation.Text = _context.Organisation;
            if (_context.Bio!=null)
            {
                Picker_Gender.SelectedItem = _context.Bio;
            }
            if (_context.DateOfBirth != null)
            {
                DatePickerDOB.Date = _context.DateOfBirth;
            }
            lblName2.Text = string.IsNullOrWhiteSpace(_context.Name) ? "25" : (25 - _context.Name.Length).ToString();
            lblName4.Text = string.IsNullOrWhiteSpace(_context.Bio) ? "150" : (150 - _context.Bio.Length).ToString();
            // _context.ProfileImageLink = _context.ProfileImageLink;
            if (check(_context.ProfileImageLink))
            {
                Uri uri = new Uri(_context.ProfileImageLink);
                var a = UriImageSource.FromUri(uri);

                ProfilePic.Source = a;
            }
            Loading.IsRunning = true;
            Services.WebService webService = new WebService();
            var li = await webService.UserService.GetUserIntrestsAsync();
            foreach(var item in li)
            {
                ServerInterests.Add(item);
            }
            lblName6.ItemsSource = ServerInterests;
            if (check(_context.Sector))
            {
                lblName6.SelectedItem = _context.Sector;
            }
            Loading.IsRunning = false;
            //if (_context.Type != "Professional")
            //{
                //Uri uri = new Uri(_context.ProfileImageLink);
                //var a = UriImageSource.FromUri(uri);

                //ProfilePic.Source = a;
            //}
            //else
            //{
            //    ProfilePic.Source = _context.LinkedInProfileImage.Source;
            //}
        }
        private async void Btnqwed_Clicked(object sender, EventArgs e)
        {
            Loading.IsRunning = true;
            

            if (_context.ProfileImage == null && _context.ProfileImageLink == null)
            {
                await DisplayAlert("Error", "Please select an image", "Okay");
                Loading.IsRunning = false;
            }
            else if ((DateTime.Now.Subtract(_context.DateOfBirth).TotalDays)/365<18)
            {
                await DisplayAlert("Error", "You must be 18 to use this app", "Okay");
                Loading.IsRunning = false;
            }
            else
            {
                UserResources userResources = new UserResources();
                WebService webService = new WebService();

                try
                {
                    if (_context.ProfileImage != null)
                    {
                        string link = await userResources.UploadImage(_context.ProfileImage.GetStream(), _context.ProfileImage.Path);
                        _context.ProfileImageLink = link.Substring(1, link.Length - 2);
                    }

                    _context.Name = lblName1.Text;
                    _context.Designation = lblName5.Text;
                    _context.Organisation = Organisation.Text;
                    _context.Bio = Picker_Gender.SelectedItem.ToString();
                    _context.DateOfBirth = DatePickerDOB.Date;
                    User user = _context.GetUser();
                    if (user.Id == null)
                    {
                        user = await webService.UserService.ApiUserAddPostAsync(user);
                    }
                    else
                    {
                        await webService.UserService.ApiUserUpdatePutAsync(user);
                    }
                    if (user.Id != null)
                    {
                        _context = new ProfileViewModel(user);
                        _cachingService.PutCache("User", JsonConvert.SerializeObject(_context));

                        Settings.UserProfileCreated = "yes";
                    }

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Okay");
                    Loading.IsRunning = false;
                }
                Loading.IsRunning = false;
                await Navigation.PushAsync(new ProfilePage());
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            MediaFile media = await CrossMedia.Current.PickPhotoAsync();
            if (media != null)
            {
                ProfilePic.Source = media.Path;


                _context.ProfileImage = media;
            }
        }

        private void lblName6_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           // interests = new List<string>();

            var a = sender as CollectionView;
            if (a.SelectedItem != null)
            {
                _context.Sector = a.SelectedItem.ToString();
            }
            if (a.SelectedItems?.Count >= 5)
            {
                a.SelectedItems.Remove(a.SelectedItems[a.SelectedItems.Count - 1]);

            }

            if (a.SelectedItems?.Count > 0)
            {
                _context.Interests = new List<string>();
                foreach(var v in a.SelectedItems)
                {
                    _context.Interests.Add(v.ToString());

                }
                //_context.Interests = a.SelectedItems as List<string>;
                _context.Interest = "";
                foreach(string s in _context.Interests)
                {
                    _context.Interest += s+" , ";
                }
               _context.Interest= _context.Interest.Substring(0, _context.Interest.Length - 2);
            }


        }

        private async void lblName5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(lblName5.Text.Length>150)
            {
                await DisplayAlert("Error", "Bio can not exceed 150 characters", "Okay");
                lblName5.Text = lblName5.Text.Substring(0, lblName5.Text.Length - 1);

            }
            
            lblName4.Text = (150 - lblName5.Text.Length).ToString();
           
        }

        private async void lblName1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lblName1.Text.Length > 25)
            {
                await DisplayAlert("Error", "Name can not exceed 25 characters", "Okay");
                lblName1.Text = lblName1.Text.Substring(0, lblName1.Text.Length - 1);

            }

            lblName2.Text = (25 - lblName1.Text.Length).ToString();
        }
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void SearchLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var list = lblName6.ItemsSource as ObservableCollection<string>;
                foreach (var li in list)
                {
                    var lol = li.Substring(0, SearchLabel.Text.Length);

                    if ( string.Equals(li.Substring(0, SearchLabel.Text.Length).ToUpper(), SearchLabel.Text.ToUpper()))
                    {
                        lblName6.ScrollTo(li);
                    }
                }
            }
            catch (Exception ex)
            { 
            }
           
        }
        private bool check(string param)
        {
            if(param==null||param=="string")
            {
                return false;
            }
            else
            {
                return true;
            }


        }
    }
}