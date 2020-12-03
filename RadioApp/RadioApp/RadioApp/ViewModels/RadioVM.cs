using RadioApp.DAL;
using RadioApp.Models;
using RadioApp.Services;
using RadioApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        private int _starImageSize;
        public int StarImageSize { get => _starImageSize; set { _starImageSize = value; OnPropertyChanged(); } }

        private bool _isRefreshing = false;
        public bool IsRefreshing { get => _isRefreshing; set { _isRefreshing = value; OnPropertyChanged(); } }

        private List<RadioStation> _radioStations;
        public List<RadioStation> RadioStations { get => _radioStations; set { _radioStations = value; OnPropertyChanged(); } }

        private RadioStation _radioStation;
        public RadioStation RadioStation { get => _radioStation; set { _radioStation = value; OnPropertyChanged(); } }

        public Command ShowAudioCommand { get; }
        public Command RefreshCommand { get; }
        public Command logOutCMD { get; }
        public RadioVM()
        {

            StartOptions();
            logOutCMD = new Command(async () =>
            {
                Application.Current.Properties["name"] = null;
                Application.Current.Properties["key"] = null;
                Application.Current.Properties.Clear();
                await Application.Current.SavePropertiesAsync();
                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new LoginPage()));
            });
            ShowAudioCommand = new Command(async () =>
            {
                //Navigate to new pagess
                await Application.Current.MainPage.Navigation.PushModalAsync(
                new AudioPage(new AudioVM(RadioStation)), true);
                IsBusy = false;
            });
            RefreshCommand = new Command( () =>
            {
                IsRefreshing = true;
                 GetAllRadioStationsFromAPI();
                IsRefreshing = false;
            });

            //Subscribe to a MessagingCenter Channel
            MessagingCenter.Subscribe<object, RadioStation>(this, "UpdateFavorite", (sender, arg) => {
                RadioStations.Find(x => x.Slug == arg.Slug).Favorite = arg.Favorite;
            });
        }
        //Start up options
        private void StartOptions()
        {
            api = new API();
            RadioStations = new List<RadioStation>();
            IsBusy = false;
            GetAllRadioStationsFromAPI();

            //Platform specific code
            if (Device.RuntimePlatform == Device.Android)
            {
                StarImageSize = 10;


            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                StarImageSize = 10;



            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                 StarImageSize = 20;

            }
            //Testing API
            // api.GetRadioStationsTest();

        }

        //Get all Radio stations from Extern API
        public async void GetAllRadioStationsFromAPI()
        {

            IsLoading = true;
            try
            {
                RadioStations = await api.GetRadioStations();
                List<Favorite> FavoriteSlugs = await api.GetFavorites();
                if(FavoriteSlugs.Count > 0)
                {
                    for (int i = 0; i < FavoriteSlugs.Count; i++)
                    {
                        for (int x = 0; x < RadioStations.Count; x++)
                        {
                            if(RadioStations[x].Slug == FavoriteSlugs[i].Slug)
                            {
                                RadioStations[x].Favorite = true;

                            }
                        }
                    }
                }
                //send stations list to Menu
                MessagingCenter.Send<object, List<RadioStation>>(this, "GetAllStations", RadioStations);
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
