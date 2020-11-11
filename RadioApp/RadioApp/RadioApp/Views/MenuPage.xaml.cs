using RadioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RadioApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MenuVM menuVM;
        public MenuPage()
        {

            Title = "Menu";
            
            InitializeComponent();

            menuVM = new MenuVM();
            BindingContext = menuVM;
           
        }

        private void FavoriteView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            menuVM.ShowFavoriteCommand.Execute(null);
        }
    }
}