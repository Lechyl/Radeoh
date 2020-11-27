using RadioApp.Models;
using RadioApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Xamarin.Forms;

namespace RadioApp.DAL
{
    public class MySqlDatabase : IDatabase
    {
        private string constring = "Data Source=radiodb-instance-1.cn2dn4x7sadv.us-east-1.rds.amazonaws.com; Initial Catalog =Radio; User ID =admin; Password =Bu$F0rrud3";
       
        public Task<bool> BulkSaveFavorites(Account account)
        {
            //Get favorite from sqlite

            //Compare favorites with Database favorites to see if it exist there, else insert into database
            //use favorite slug from the sqlite favorites and add them to account_has_favorite table with the account id
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFavorite(RadioStation station)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Favorite>> GetFavorites()
        {
            List<Favorite> list = new List<Favorite>();

            try
            {
                if (Application.Current.Properties.ContainsKey("key"))
                {

                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                    conn.Open();

                    string query = "SELECT slugFavorite FROM Radio.Account_has_favorites where idAccount = @id";
                    using (MySqlCommand cmd = new MySqlCommand(query,conn))
                    {
                        cmd.Parameters.AddWithValue("@id", Application.Current.Properties["key"].ToString());

                        MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                        if (reader.HasRows)
                        {
                            while(await reader.ReadAsync())
                            {
                                
                                list.Add(new Favorite() { Slug = reader.GetString(0)});
                           
                            }
                        }
                        reader.Close();

                    }
                    await conn.CloseAsync();
                }
                }

                return list;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<bool> Login(Account account)
        {

            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                try
                {
                    conn.Open();

                }
                catch (Exception)
                {

                    throw;
                }

                string query = "select COUNT(*) as accounts,id from Account where username = @username and password = @password ";
                using (var cmd = new MySqlCommand(query, conn))
                {


                    cmd.Parameters.AddWithValue("@username", account.Username);
                    cmd.Parameters.AddWithValue("@password", account.Password);

                    try
                    {
                        MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if ( reader.HasRows)
                        {

                            while (await reader.ReadAsync())
                            {
                               if (reader.GetInt32(0) == 1)
                                {
      

                                    Application.Current.Properties["name"] = account.Username;
                                    Application.Current.Properties["key"] = reader.GetInt32(1);
                                    await Application.Current.SavePropertiesAsync();

                                    reader.Close();
                                    await conn.CloseAsync();
                                    return true;
                                }

                            }
                        }
                        reader.Close();
                        await conn.CloseAsync();

                        return false;

                    }
                    catch (Exception)
                    {
                        await conn.CloseAsync();
                        return false;
                    }
                }


            }
        }

        public async Task<bool> Register(Account account)
        {
            try
            {


                //Check if the Username exist in the database #Yes go on #No return and send back false boolean
                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                    conn.Open();
                    string query = "select COUNT(*) as accounts from Account where username = @username";

                    using (MySqlCommand cmd = new MySqlCommand(query,conn))
                    {
                        cmd.Parameters.AddWithValue("@username", account.Username);
                        MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            while(await reader.ReadAsync())
                            {
                                if(reader.GetInt16(0) != 0)
                                {
                                    reader.Close();
                                    await conn.CloseAsync();

                                    return false;
                                }
 
                            }
                        }
                        else
                        {
                            reader.Close();
                            await conn.CloseAsync();

                            return false;

                        }

                        reader.Close();

                    }
                    query = "INSERT INTO Radio.Account (email,username,password,enabled) VALUES (@email,@username,@password,@enabled)";
                    //Create User
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", account.Email);
                        cmd.Parameters.AddWithValue("@username", account.Username);
                        cmd.Parameters.AddWithValue("@password", account.Password);
                        cmd.Parameters.AddWithValue("@enabled", 1);
                        cmd.ExecuteNonQuery();
                    }

                   await conn.CloseAsync();
                }
                return true;
            }

            catch (Exception)
            {

                return false;

            }
        }

        public Task<bool> SaveFavorite(RadioStation station)
        {
            return Task.FromResult(true);
        }
    }
}
