using RadioApp.DAL;
using RadioApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RadioApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MasterDetailPage
            {
                Master = new MenuPage(),
                Detail = new NavigationPage(new RadioPage())
            };
        }
        static SqliteDatabase database;
        public static SqliteDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new SqliteDatabase();
                }
                return database;
            }
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
