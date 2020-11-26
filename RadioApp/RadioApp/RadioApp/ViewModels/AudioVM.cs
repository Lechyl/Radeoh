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
using System.Threading.Tasks;

namespace RadioApp.ViewModels
{
    public class AudioVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public MySqlDatabase db;
        private bool _isPlaying;
        public bool IsPlaying { get => _isPlaying; set { _isPlaying = value; OnPropertyChanged(); } }

        private bool _isFavorite;
        public bool IsFavorite { get => _isFavorite; set { _isFavorite = value; OnPropertyChanged(); } }

        private string _notificationText;
        public string NotificationText { get => _notificationText; set { _notificationText = value; OnPropertyChanged(); } }

        private bool _isNotification = false;
        public bool IsNotification { get => _isNotification; set { _isNotification = value; OnPropertyChanged(); } }

        public Command BackCommand { get; }
        public Command AddFavorite { get; }
        public Command RemoveFavorite { get; }
        public Command DisableNotificationCommand { get; }
        private RadioStation _radioStation;
        public RadioStation RadioStation { get => _radioStation; set { _radioStation = value; OnPropertyChanged(); } }

        public AudioVM(RadioStation station)
        {
            //Init statements
            StartOptions(station);


            BackCommand = new Command(async () => {
                Stop();
                //Pop Modal
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });

            AddFavorite = new Command(async () => {
                await db.SaveFavorite(RadioStation);
                IsFavorite = true;
                RadioStation.Favorite = true;
                ShowNotification("Radio Channel Added To Favorite");
                
                //Send Message to a MessagingCenter Channel
                MessagingCenter.Send<object, RadioStation>(this, "UpdateFavorite", RadioStation);
            });

            RemoveFavorite = new Command(async () =>
            {
                await db.DeleteFavorite(RadioStation);
                IsFavorite = false;
                RadioStation.Favorite = false;
                ShowNotification("Radio Channel Removed From Favorite");

                //Send Message to a MessagingCenter Channel
                MessagingCenter.Send<object, RadioStation>(this, "UpdateFavorite", RadioStation);

            });
            DisableNotificationCommand = new Command(() =>
           {
               HideNotification();
           });


        }

        //Start Up Options
        public void StartOptions(RadioStation station)
        {
            db = new MySqlDatabase();
            IsPlaying = false;
            RadioStation = station;
            IsFavorite = station.Favorite;
            //Stop all background radio music if closed wrong
            Stop();
            Play();

        }

        //Show Notification popup
        private void ShowNotification(string message)
        {
            NotificationText = message;
            IsNotification = true;
        }
        //Hide Notification Popup
        private void HideNotification()
        {
            IsNotification = false;

        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //Invoke/Raise Event of the specific method name

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }


        //Play Music/Radio
        public async void Play()
        {

            await CrossMediaManager.Current.Play(RadioStation.StreamUrl);
            IsPlaying = true;
        }



        //Stop Music/Radio
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
