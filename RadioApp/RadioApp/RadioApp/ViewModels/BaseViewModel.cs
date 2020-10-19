using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;


namespace RadioApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /*     public IApiManager ApiManager;
             IApiService<IRadeohApi> radeohApi = new ApiService<IRadeohApi>(Config.RadeohBaseUrl);

             public bool IsBusy { get; set; }

             public BaseViewModel()
             {
                 ApiManager = new ApiManager(radeohApi);
             }

             public async Task RunSafe(Task task, bool ShowLoading = true, string loadingMessage = null)
             {
                 try
                 {
                     if (IsBusy) return;

                     IsBusy = true;

                     if (ShowLoading) UserDialogs.Instance.ShowLoading(loadingMessage ?? "Loading");

                     await task;
                 }
                 catch (Exception e)
                 {
                     IsBusy = false;
                     UserDialogs.Instance.HideLoading();
                     Debug.WriteLine(e.ToString());
                     await App.Current.MainPage.DisplayAlert("Error", "Check your internet connection", "Ok");
                 }
                 finally
                 {
                     IsBusy = false;
                     if (ShowLoading) UserDialogs.Instance.HideLoading();
                 }
        */
             }
    }
