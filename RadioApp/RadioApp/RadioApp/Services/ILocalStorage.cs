using RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RadioApp.Services
{
    interface ILocalStorage
    {
          Task<List<RadioStation>> GetFavorites();

          Task<int> SaveFavorite(RadioStation station);

          Task<int> DeleteFavorite(RadioStation station);
    }
}
