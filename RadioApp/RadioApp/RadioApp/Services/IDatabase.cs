using RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RadioApp.Services
{
    interface IDatabase
    {

        Task<List<Favorite>> GetFavorites();

        Task<bool> SaveFavorite(RadioStation station);

        Task<bool> DeleteFavorite(RadioStation station);

        Task<bool> Login(Account account);
        Task<bool> Register(Account account);

        //Only supported the next 3 months after start date.
        Task<bool> BulkSaveFavorites(Account account);

    }
}
