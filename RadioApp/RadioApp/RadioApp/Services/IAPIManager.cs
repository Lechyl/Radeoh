using RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace RadioApp.Services
{
    interface IAPIManager
    {
        Task<ObservableCollection<RadioStation>> GetRadioStations();
    }
}
