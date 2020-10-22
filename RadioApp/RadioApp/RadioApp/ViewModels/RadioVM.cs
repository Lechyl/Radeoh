using RadioApp.Models;
using RadioApp.Services;
using RadioApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RadioApp.ViewModels
{
    class RadioVM : INotifyPropertyChanged
    {

        public API api;

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsBusy { get; set; }
         
        private bool _isLoading = false;
        public bool IsLoading 
        { 
            get => _isLoading; 
            set 
            { 
                _isLoading = value;
                OnPropertyChanged();
            }
            
        }

        private List<RadioStation> _radioStations;
        public List<RadioStation> RadioStations { get => _radioStations; set { _radioStations = value; OnPropertyChanged(); } }

        private RadioStation _radioStation;
        public RadioStation RadioStation { get => _radioStation; set { _radioStation = value; OnPropertyChanged(); } }

        public Command ShowAudioCommand { get; }

        public RadioVM()
        {
            api = new API();
            RadioStations = new List<RadioStation>();
            IsBusy = false;

            ShowAudioCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(
                new AudioPage(new AudioVM(RadioStation)),true);
                IsBusy = false;
            });
        }


        public async void GetAllRadioStationsFromAPI()
        {

            IsLoading = true;
            try
            {
                RadioStations = await api.GetRadioStations();

            }
            catch (Exception)
            {
                RadioStations = null;
            }
            IsLoading = false;

        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //Invoke/Raise Event of the specific method name

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
