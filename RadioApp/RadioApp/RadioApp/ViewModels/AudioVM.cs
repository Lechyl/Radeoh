using RadioApp.Models;
using RadioApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using MediaManager;

namespace RadioApp.ViewModels
{
    public class AudioVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isPlaying;
        public bool IsPlaying { get => _isPlaying; set { _isPlaying = value; OnPropertyChanged(); } }
        public Command StopPlayCommand { get; }
        public Command BackCommand { get; }
        private RadioStation _radioStation;
        public RadioStation RadioStation { get => _radioStation; set { _radioStation = value; OnPropertyChanged(); } }

        public AudioVM(RadioStation station)
        {
            IsPlaying = false;
            RadioStation = station;

            //Stop all background radio music if closed wrong
            Stop();
            BackCommand = new Command(async () => {
                Stop();
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });


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

        public async void Stop()
        {
            if (IsPlaying)
            {

                IsPlaying = false;
                await CrossMediaManager.Current.Stop();
            }

        }
    }
}
