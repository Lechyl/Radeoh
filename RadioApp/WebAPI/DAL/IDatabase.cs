using WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.DAL
{
    interface IDatabase
    {
        public Task<Account> GetByID(int id);

        Task<List<Favorite>> GetFavorites(int id);

        Task<bool> SaveFavorite(int id,Favorite station);

        Task<bool> DeleteFavorite(int id,Favorite station);

        Task<DtoAccount> Login(DtoAccount account);
        Task<DtoAccount> Register(Account account);

        //Only supported the next 3 months after start date.
        Task<bool> BulkSaveFavorites(int id,List<Favorite> favorites);

    }
}
