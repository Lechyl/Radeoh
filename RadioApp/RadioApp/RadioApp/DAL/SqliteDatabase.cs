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

        public static List<Favorite> FavoriteList { get; set; }
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public SqliteDatabase()
        {
            FavoriteList = new List<Favorite>();
            InitializeAsync().SafeFireAndForget(false);

            Database.CreateTableAsync<Favorite>();
           
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Favorite).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Favorite)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public async Task<List<Favorite>> GetFavorites()
        {
            FavoriteList = await Database.Table<Favorite>().ToListAsync();
            return FavoriteList;
        }

        public async Task<int> SaveFavorite(RadioStation station)
        {
            Favorite favorite = new Favorite();
            favorite.Slug = station.Slug;
            if(FavoriteList.Count > 0)
            {
                if (!FavoriteList.Exists(x => x.Slug == station.Slug))
                {
                    return await Database.InsertAsync(favorite);

                }
            }
            else
            {
                
                return await Database.InsertAsync(favorite);

            }

            return 0;
        }

        public async Task<int> DeleteFavorite(RadioStation station)
        {
            FavoriteList.RemoveAll(x => x.Slug == station.Slug);
            Favorite favorite = new Favorite();
            favorite.Slug = station.Slug;
            return await Database.DeleteAsync(favorite);
        }
    }
}
