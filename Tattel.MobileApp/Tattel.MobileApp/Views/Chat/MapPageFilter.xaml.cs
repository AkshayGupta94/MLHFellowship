using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattel.MobileApp.Model;
using Tattel.MobileApp.Services;
using Tattel.MobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tattel.MobileApp.Views.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPageFilter : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ObservableCollection<string> _ServerInterests;
        
        public MapFilterQuery query;
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
      
        public MapPageFilter()
        {
            InitializeComponent();
            loadPage();
          
        }
        async void loadPage()
        {
            loading.IsRunning = true;
            ServerInterests = new ObservableCollection<string>();
            query = new MapFilterQuery();
            Services.WebService webService = new WebService();
            var li = await webService.UserService.GetUserIntrestsAsync();
            foreach (var item in li)
            {
                ServerInterests.Add(item);
            }
            lblName6.ItemsSource = ServerInterests;
            loading.IsRunning = false;
        }
        private async void btnMain_Clicked(object sender, EventArgs e)
        {
            query.Designation = lblAbc.Text;
            query.Organisation = lblAbc1.Text;
            query.Gender = Picker_Gender.SelectedItem?.ToString();
            query.Age = EntryAge.Text;
            int i=0;
            if(int.TryParse((query.Age), out i)||query.Age==null)
            {
                Navigation.PushAsync(new MapPage());

            }
            else
            {
                await DisplayAlert("Bad input", "Please input a valid number for age", "Okay");

            }
        }

        private void SearchLabel_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var list = lblName6.ItemsSource as ObservableCollection<string>;
                foreach (var li in list)
                {
                    var lol = li.Substring(0, SearchLabel.Text.Length);

                    if (string.Equals(li.Substring(0, SearchLabel.Text.Length).ToUpper(), SearchLabel.Text.ToUpper()))
                    {
                        lblName6.ScrollTo(li);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void lblName6_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = sender as CollectionView;
            if (a.SelectedItem != null)
            {
               query.Sector = a.SelectedItem.ToString();
            }
        }
    }
}