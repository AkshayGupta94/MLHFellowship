using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tattel.MobileApp.CustomUI;
using Tattel.MobileApp.Model;
using Tattel.MobileApp.Views.Profile;
using Tattel.WebApiClient.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;


namespace Tattel.MobileApp.ViewModels.Chat
{
    public class MapPageViewModel : BaseViewModel
    {
        ProfileViewModel _context;
        public event EventHandler PinTapped;
        public string SenderId { get; set; }
        INavigation navigationService;
        public List<WebApiClient.Model.User> UserList { get; set; }
        private CustomUI.CustomMap _Map { get; set; }
        public double MaxAge { get; set; }
        public double MinAge { get; set; }
        public MapFilterQuery mapFilterQuery { get; set; }
        public CustomUI.CustomMap Map 
        {
            get
            {
                return _Map;
            }
            set
            {
                _Map = value;
                OnPropertyChanged();
            }
        }
        private Location _MyLocation;
        public Location MyLocation
        {
            get
            {
                return _MyLocation;
            }
            set
            {
                _MyLocation = value;
               
                
            }
                }
        public MapPageViewModel(INavigation navigation)
        {
            this.navigationService = navigation;
            //Map = new Xamarin.Forms.Maps.Map();
        }
        public async Task LoadData()
        {
            try
            {
                 _context = JsonConvert.DeserializeObject<ProfileViewModel>((string)Application.Current.Properties["User"]);
                MaxAge = 100;MinAge = 18;
                await SetMap();
            }
            catch (Exception ex)
            { 
            
            }
        }
        CancellationTokenSource cts;

        async Task<Location> GetCurrentLocation()
        {
            try
            {
               
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, cts.Token);

                if (location != null)
                {
                    return location;
                }
                else
                    return new Location();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            return new Location();
        }
        public async Task SetMap()
        {
            try
            {
                IsBusy = true;
                LoadingStatus = "Getting Location";
                MyLocation = await GetCurrentLocation();
                Services.WebService web = new Services.WebService();
                LoadingStatus = "Updating Your Location on servers";

                await web.UserService.ApiUserUpdateLocationUpdateLocationPutAsync(_context.Id, MyLocation.Latitude, MyLocation.Longitude);
                LoadingStatus = "Loggin you in to our matching system";

                await web.UserService.ApiUserUpdateStatusUpdateStatusPutAsync(_context.Id, true);
                LoadingStatus = "Finding people around you";

                UserList = await web.UserService.ApiUserGetNearbyUsersGetNearbyUsersGetAsync(_context.Id, 10, 500);

                Position position = new Position(MyLocation.Latitude, MyLocation.Longitude);
                MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);


                List<CustomPin> customPins = new List<CustomPin>();
                List<Pin> pins = new List<Pin>();
                CustomPin temp = new CustomPin { Label = "Me",  Sector = _context.Sector, Role = _context.Designation, Organisation = _context.Organisation, Position = position, Type = PinType.Place, ImageUrl = _context.ProfileImageLink, Id = _context.Id };
                customPins.Add(temp);
                pins.Add(temp);
                foreach (WebApiClient.Model.User user in UserList)
                {
                    if (_context.Type==1)//if logged in user is student
                    {
                       if(user.UserType==2)//add only proofessionals
                        {
                            if (checkFilter(user))
                            {
                                temp = new CustomPin { Label = user.Sector, Sector = user.Sector, Role = user.Role, Organisation = user.Company, Id = user.Id, Position = new Position((double)user.Latitude, (double)user.Longitude), Type = PinType.SearchResult, ImageUrl = user.ProfilePicPath };
                                customPins.Add(temp);
                                temp.InfoWindowClicked += Temp_InfoWindowClicked;
                                pins.Add(temp);
                            }
                        }
                    }
                    else 
                    {
                        if (checkFilter(user))
                        {
                            temp = new CustomPin { Label = user.Sector, Sector = user.Sector, Role = user.Role, Organisation = user.Company, Id = user.Id, Position = new Position((double)user.Latitude, (double)user.Longitude), Type = PinType.SearchResult, ImageUrl = user.ProfilePicPath };
                            customPins.Add(temp);
                            temp.InfoWindowClicked += Temp_InfoWindowClicked;
                            pins.Add(temp);
                        }
                    }

                }
                Map = new CustomUI.CustomMap(mapSpan) { CustomPins = customPins };
                foreach (Pin p in pins)
                {
                    Map.Pins.Add(p);

                }
            }
            catch (Exception ex)
            {
                LoadingStatus = ex.Message+" kindly reload the page";
            }
            IsBusy = false;
        }

        private async void Temp_InfoWindowClicked(object sender, PinClickedEventArgs e)
        {
            var a = UserList.First(a => a.Id == ((CustomPin)sender).Id);
            SenderId = a.Id;
            EventHandler handler = PinTapped;
            handler?.Invoke(this, new EventArgs());

           // navigationService.PushAsync(new ProfilePage(((CustomPin)sender).Id));

        }
        private bool checkFilter(User user)
        {
            if (mapFilterQuery == null)
            {
                return true;
            }
            else
            {
                if(string.IsNullOrEmpty(mapFilterQuery.Designation))
                {
                    if(user.Role!= mapFilterQuery.Designation)
                    {
                        //return false;
                    }
                }
                if (string.IsNullOrEmpty(mapFilterQuery.Gender))
                {
                    if (user.Bio != mapFilterQuery.Gender)
                    {
                        //return false;
                    }
                }
                if (string.IsNullOrEmpty(mapFilterQuery.Organisation))
                {
                    if (user.Company != mapFilterQuery.Organisation)
                    {
                        //return false;
                    }
                }
                if (string.IsNullOrEmpty(mapFilterQuery.Sector))
                {
                    if (user.Sector != mapFilterQuery.Sector)
                    {
                        //return false;
                    }
                }
                if (string.IsNullOrEmpty(mapFilterQuery.Age))
                {                 
                    if ((DateTime.Now.Subtract(new DateTime(long.Parse(user.DateOfBirth))).TotalDays / 365) < int.Parse(mapFilterQuery.Age))
                    {
                        //return false;
                    }
                }
                return true;
            }
        }


    }
}
