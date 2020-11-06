using RadioApp.Converter;
using RadioApp.DAL;
using RadioApp.Models;
using RadioApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace RadioApp.ViewModels
{
    class MenuVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Favorite SelectedFavorite { get; set; }

        private ObservableCollection<Favorite> _favorites;
        public ObservableCollection<Favorite> Favorites 
        {
            get => _favorites; 
            set 
            { 
                _favorites = value; 
                OnPropertyChanged(); 
            } 
        }

        public Command ShowFavoriteCommand { get; }

        SqliteDatabase sqliteDatabase;
        public MenuVM()
        {
            StartOptions();

            ShowFavoriteCommand = new Command(async() =>
            {
                RadioStation radioStation = Mapper.Map(SelectedFavorite);
                await Application.Current.MainPage.Navigation.PushModalAsync(new AudioPage(new AudioVM(radioStation)), true);
            });

            MessagingCenter.Subscribe<object, RadioStation>(this, "UpdateFavorite", (sender, arg) => 
            {

                if (arg.Favorite)
                {
                    Favorites.Add(Mapper.Map(arg));

                }
                else
                {
                    Favorites.Remove(Favorites.Where(x => x.Slug == arg.Slug).Single());

                }

            });
        }
        private async void StartOptions()
        {
            sqliteDatabase = new SqliteDatabase();
            Favorites = new ObservableCollection<Favorite>(await sqliteDatabase.GetFavorites());
           



        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            //Invoke/Raise Event of the specific method name

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
