using RadioApp.DAL;
using RadioApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace RadioApp.ViewModels
{
    class RegisterVM : INotifyPropertyChanged
    {
        public Command registerCMD { get; }
        public Command goBackCMD { get; }
        private Account _user;
        public Account User { get => _user; set { _user = value; OnPropertyChanged(); } }

        private string _confirmPassword;
        public string ConfirmPassword { get => _confirmPassword; set { _confirmPassword = value; OnPropertyChanged(); } }

        private string _errorText;
        public string ErrorText { get => _errorText; set { _errorText = value; OnPropertyChanged(); } }
        private bool _errorDisplay;
        public bool ErrorDisplay { get => _errorDisplay; set { _errorDisplay = value; OnPropertyChanged(); } }
        private bool saveData { get; set; }
        public MySqlDatabase db;

        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterVM()
        {
            StartUpOptions();

            registerCMD = new Command(async () =>
            {

                if (!string.IsNullOrWhiteSpace(User.Email) && !string.IsNullOrWhiteSpace(User.Username) && !string.IsNullOrWhiteSpace(User.Password) && !string.IsNullOrWhiteSpace(ConfirmPassword))
                {

                    if (User.Password != ConfirmPassword)
                    {
                        ErrorDisplay = true;
                        ErrorText = "Den nye adgangskode passer ikke med gentag ny adgangskoden";

                    }
                    else
                    {

                        bool success = await db.Register(User);
                        if (success)
                        {
                            saveData = await Application.Current.MainPage.DisplayAlert("Vigtig!", "Vil du gerne gemme nuværende favoritter og indstiller på kontoen?", "Ja", "Nej");

                            if (saveData)
                            {
                                await db.BulkSaveFavorites(User);
                            }

                        }
                        else
                        {
                            ErrorDisplay = true;
                            ErrorText = "Brugernavnet findes allerede. Vælg et andet brugernavn";
                        }

                    }
                }
                else
                {
                    ErrorDisplay = true;
                    ErrorText = "Nogle af felterne er tomme";
                }



            });

            goBackCMD = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });
        }

        private void StartUpOptions()
        {
            db = new MySqlDatabase();
            User = new Account();
            saveData = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //Invoke/Raise Event of the specific method name

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
