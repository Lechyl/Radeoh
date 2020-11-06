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
    public partial class AudioPage : ContentPage
    {
        AudioVM audioVM;


        public AudioPage(AudioVM audioViewModel)
        {
        
            InitializeComponent();

            audioVM = audioViewModel;

            BindingContext = audioVM;


        }
        /*   public AudioPage()
           {
               Title = "Audio Player";

               InitializeComponent();

               audioVM = new AudioVM();
               audioVM.RadioStation.Title = "hello";
               BindingContext = audioVM;


           }*/

        //only Works on Android
        protected override bool OnBackButtonPressed()
        {
            audioVM.BackCommand.Execute(null);
            return true;
        }
    }
}