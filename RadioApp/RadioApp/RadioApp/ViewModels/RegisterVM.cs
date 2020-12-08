using RadioApp.DAL;
using RadioApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using RadioApp.Helper;
using RadioApp.Services;

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
        private bool _registerLoading;
        public bool RegisterLoading { get => _registerLoading; set { _registerLoading = value; OnPropertyChanged(); } }
        private bool saveData { get; set; }
        public API api;

        public event PropertyChangedEventHandler PropertyChanged;

        public RegisterVM()
        {
            StartUpOptions();

            registerCMD = new Command(async () =>
            {
                RegisterLoading = true;
                if (!string.IsNullOrWhiteSpace(User.Email) && !string.IsNullOrWhiteSpace(User.Username) && !string.IsNullOrWhiteSpace(User.Password) && !string.IsNullOrWhiteSpace(ConfirmPassword))
                {

                    if (!RegexUtility.IsValidEmail(User.Email))
                    {
                        ErrorDisplay = true;
                        ErrorText = "Email addressen er ugyldig";

                    }
                    else
                    {

                        if (User.Password != ConfirmPassword)
                        {
                            ErrorDisplay = true;
                            ErrorText = "Den nye adgangskode passer ikke med gentag ny adgangskoden";

                        }
                        else
                        {

                            var account = await api.Register(User);
                            RegisterLoading = false;

                            if (account != null)
                            {
                                ErrorDisplay = false;
                                saveData = await Application.Current.MainPage.DisplayAlert("Vigtig!", "Vil du gerne gemme nuværende favoritter og indstiller på kontoen?", "Ja", "Nej");

                                if (saveData)
                                {
                                    await api.BulkSaveFavorites(account);
                                }
                                Application.Current.Properties["tmpID"] = null;
                                await Application.Current.SavePropertiesAsync();
                                await Application.Current.MainPage.DisplayAlert("Du er nu blevet registreret", "Tryk OK for at logge ind", "OK");

                                goBackCMD.Execute(null);
                            }
                            else
                            {
                                ErrorDisplay = true;
                                ErrorText = "Brugernavnet findes allerede. Vælg et andet brugernavn";
                            }

                        }
                    }

                }
                else
                {
                    ErrorDisplay = true;
                    ErrorText = "Nogle af felterne er tomme";
                }
                RegisterLoading = false;


            });

            goBackCMD = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });
        }

        private void StartUpOptions()
        {
            api = new API();
            User = new Account();
            saveData = false;
            RegisterLoading = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //Invoke/Raise Event of the specific method name

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
