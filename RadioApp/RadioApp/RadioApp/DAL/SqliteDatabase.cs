using SQLite;
using System;
using System.Linq;
using System.Threading.Tasks;
using RadioApp.Models;
using RadioApp.Extensions;
using System.Collections.Generic;
using RadioApp.Services;
using System.Collections.ObjectModel;

namespace RadioApp.DAL
{
    public class SqliteDatabase : ILocalStorage
    {

        public static List<RadioStation> FavoriteList { get; set; }
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public SqliteDatabase()
        {
            FavoriteList = new List<RadioStation>();
            InitializeAsync().SafeFireAndForget(false);

            Database.CreateTableAsync<RadioStation>();
           
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(RadioStation).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(RadioStation)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public async Task<List<RadioStation>> GetFavorites()
        {
            FavoriteList = await Database.Table<RadioStation>().ToListAsync();
            return FavoriteList;
        }

        public async Task<int> SaveFavorite(RadioStation station)
        {
            if(!FavoriteList.Exists(x => x.Slug == station.Slug))
            {
                return await Database.InsertAsync(station.Slug);

            }
            return 0;
        }

        public async Task<int> DeleteFavorite(RadioStation station)
        {
            FavoriteList.RemoveAll(x => x.Slug == station.Slug);
            return await Database.DeleteAsync(station);
        }
    }
}
