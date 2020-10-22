using RadioApp.Models;
using RadioApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace RadioApp.ViewModels
{
    public class AudioVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Command StopPlayCommand { get; }
        public Command BackCommand { get; }
        private RadioStation _radioStation;
        public RadioStation RadioStation { get => _radioStation; set { _radioStation = value; OnPropertyChanged(); } }

        public AudioVM(RadioStation station)
        {
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

        public void Play()
        {
            //We are going to use this interface as a Dependency Injection. This means we are going to implement it separately in each plataform: iOS and Android.
            DependencyService.Get<IStreaming>().DataSource = RadioStation.StreamUrl.ToString();

            DependencyService.Get<IStreaming>().Play();
        }

        public void Stop()
        {
            DependencyService.Get<IStreaming>().Stop();
        }
    }
}
