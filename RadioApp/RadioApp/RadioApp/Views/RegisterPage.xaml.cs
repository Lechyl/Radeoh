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
    public partial class RegisterPage : ContentPage
    {
        RegisterVM vm;
        public RegisterPage()
        {
            Title = "Registrerings Form";
            InitializeComponent();

            vm = new RegisterVM();

            BindingContext = vm;
        }
    }
}