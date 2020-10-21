using RadioApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RadioApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RadioPage : ContentPage
    {

        RadioVM radioVM;
        public RadioPage()
        {
            Title = "Radio";
            InitializeComponent();

            radioVM = new RadioVM();
 
            BindingContext = radioVM;

            radioVM.GetAllRadioStationsFromAPI();

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert(radioVM.RadioStation.Title, "hello", "Ok");
        }
    }

    
}