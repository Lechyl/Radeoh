using RadioApp.Models;
using RadioApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using MediaManager;
using RadioApp.DAL;

namespace RadioApp.ViewModels
{
    public class AudioVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public SqliteDatabase db;
        private bool _isPlaying;
        public bool IsPlaying { get => _isPlaying; set { _isPlaying = value; OnPropertyChanged(); } }


        private bool _isFavorite;
        public bool IsFavorite { get => _isFavorite; set { _isFavorite = value; OnPropertyChanged(); } }

        public Command BackCommand { get; }
        public Command AddFavorite { get; }
        public Command RemoveFavorite { get; }
        private RadioStation _radioStation;
        public RadioStation RadioStation { get => _radioStation; set { _radioStation = value; OnPropertyChanged(); } }

        public AudioVM(RadioStation station)
        {

            StartOptions(station);
            BackCommand = new Command(async () => {
                Stop();
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });
            AddFavorite = new Command(async () => {
                await db.SaveFavorite(RadioStation);
                IsFavorite = true;
                RadioStation.Favorite = true;

                MessagingCenter.Send<object, RadioStation>(this, "UpdateFavorite", RadioStation);
            });
            RemoveFavorite = new Command(async () =>
            {
                await db.DeleteFavorite(RadioStation);
                IsFavorite = false;
                RadioStation.Favorite = false;
                MessagingCenter.Send<object, RadioStation>(this, "UpdateFavorite", RadioStation);

            });


        }
        public void StartOptions(RadioStation station)
        {
            db = new SqliteDatabase();
            IsPlaying = false;
            RadioStation = station;
            IsFavorite = station.Favorite;
            //Stop all background radio music if closed wrong
            Stop();
            Play();

        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //Invoke/Raise Event of the specific method name

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public async void Play()
        {

            await CrossMediaManager.Current.Play(RadioStation.StreamUrl);
            IsPlaying = true;
        }

        public void Stop()
        {
            if (IsPlaying)
            {

                IsPlaying = false;
                CrossMediaManager.Current.Stop();
            }

        }
    }
}
