using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Tattel.MobileApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _IsBusy;
        public bool IsBusy
        {
            get
            {
                return _IsBusy;
            }
            set
            {
                _IsBusy = value;
                OnPropertyChanged();
            }
        }
        private string _LoadingStatus;
        public string LoadingStatus
        {
            get
            {
                return _LoadingStatus;
            }
            set
            {
                _LoadingStatus = value;
                OnPropertyChanged();
            }
        }
        public void OnPropertyChanged([CallerMemberName]string name="")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
