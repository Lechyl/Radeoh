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
        Task<List<RadioStation>> GetRadioStations();

        Task<List<Favorite>> GetFavorites();

        Task<bool> SaveFavorite(RadioStation station);

        Task<bool> DeleteFavorite(RadioStation station);

        Task<DtoAccount> Login(Account account);
        Task<DtoAccount> Register(Account account);

        //Only supported the next 3 months after start date.
        Task<bool> BulkSaveFavorites(DtoAccount account);
    }
}
