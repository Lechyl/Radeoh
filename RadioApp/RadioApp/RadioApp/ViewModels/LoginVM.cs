using RadioApp.DAL;
using RadioApp.Models;
using RadioApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace RadioApp.ViewModels
{
    class LoginVM : INotifyPropertyChanged
    {
        public Command goToRegisterCMD { get; }
        public Command loginCMD { get; }
        public Command goToResetPwdCMD { get; }



        private Account _user;
        public Account User { get => _user; set { _user = value; OnPropertyChanged(); } }
        
        private string _password;
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        private bool _failLogin;
        public bool FailLogin { get => _failLogin; set { _failLogin = value; OnPropertyChanged(); } }

        private MySqlDatabase db;

        public event PropertyChangedEventHandler PropertyChanged;

        public LoginVM()
        {
            
            StartUpOptions();
           goToRegisterCMD = new Command(async () =>
           {
               await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new RegisterPage()));
           });
            loginCMD = new Command(async () =>
            {
                bool success = await db.Login(User);
                if (success)
                {
                    Application.Current.MainPage = new MasterDetailPage
                    {
                        Master = new MenuPage(),
                        Detail = new NavigationPage(new RadioPage())
                        {
                            BarBackgroundColor = Color.DarkGray
                        }
                    };
                }
                else
                {
                    FailLogin = true;
                }
                //Application.Current.MainPage.DisplayAlert(User.Password, User.Username, "ok");
            });
        }

        private void StartUpOptions()
        {
            db = new MySqlDatabase();

            User = new Account();
            FailLogin = false;
            FailLogin = true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //Invoke/Raise Event of the specific method name

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
