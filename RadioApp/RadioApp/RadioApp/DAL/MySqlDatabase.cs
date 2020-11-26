using RadioApp.Models;
using RadioApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

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

        public Task<List<Favorite>> GetFavorites()
        {
            throw new NotImplementedException();

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

                string query = "select COUNT(*) as accounts from Account where username = @username and password = @password ";
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
                                    reader.Close();
                                    await conn.CloseAsync();
                                    return true;
                                }

                            }
                        }

                        return false;

                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }


            }
        }

        public Task<bool> Register(Account account)
        {
            try
            {


                //Check if the Username exist in the database #Yes go on #No return and send back false boolean
                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                    conn.Open();
                    string query = "INSERT INTO Radio.Account (email,username,password,enabled) VALUES (@email,@username,@password,@enabled)";
                    //Create User
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", account.Email);
                        cmd.Parameters.AddWithValue("@username", account.Username);
                        cmd.Parameters.AddWithValue("@password", account.Password);
                        cmd.Parameters.AddWithValue("@enabled", 1);
                        cmd.ExecuteNonQuery();
                    }


                }
                return Task.FromResult(true);
            }

            catch (Exception)
            {

                return Task.FromResult(false);

            }
        }

        public Task<bool> SaveFavorite(RadioStation station)
        {
            return Task.FromResult(true);
        }
    }
}
