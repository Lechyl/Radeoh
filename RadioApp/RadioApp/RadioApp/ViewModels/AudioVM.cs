using RadioApp.Models;
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


        public Command BackCommand { get; }
        private RadioStation _radioStation;
        public RadioStation RadioStation { get => _radioStation; set { _radioStation = value; OnPropertyChanged(); } }

        public AudioVM(RadioStation station)
        {
            RadioStation = station;

            BackCommand = new Command(async () => {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //Invoke/Raise Event of the specific method name

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

    }
}
