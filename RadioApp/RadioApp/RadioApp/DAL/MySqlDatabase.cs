using RadioApp.Models;
using RadioApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Xamarin.Forms;
using RadioApp.Helper;

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
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", Application.Current.Properties["key"].ToString());

                            MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {

                                    list.Add(new Favorite() { Slug = reader.GetString(0) });

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
                int id = 0;
                string hashedPassword = "";
                string salt = "";
                int attempt = 0;
                int enabled = 0;
                string query = "select password,salt,id,attempt,enabled from Account where username = @username";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", account.Username);
                    MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {

                        while (await reader.ReadAsync())
                        {
                            hashedPassword = reader.GetString(0);
                            salt = reader.GetString(1);
                            id = reader.GetInt32(2);
                            attempt = reader.GetInt32(3);
                            enabled = reader.GetInt16(4);
                        }
                    }
                    reader.Close();
                }


                try
                {

                    //Check if user attempted to login more than 3 times
                    if (attempt < 3 && enabled == 1)
                    {

                        if (Hash.VerifyHash(account.Password, hashedPassword, salt))
                        {
                            //Logged in
                            //reset attempt to zero
                            Application.Current.Properties["name"] = account.Username;
                            Application.Current.Properties["key"] = id;
                            await Application.Current.SavePropertiesAsync();
                            string query1 = "Update Radio.Account SET attempt = @attempt WHERE username = @username and id = @id";
                            using (MySqlCommand cmd1 = new MySqlCommand(query1, conn))
                            {
                                cmd1.Parameters.AddWithValue("@username", account.Username);
                                cmd1.Parameters.AddWithValue("@attempt", 0);
                                cmd1.Parameters.AddWithValue("@id", id);
                                await cmd1.ExecuteNonQueryAsync();

                            }
                            await conn.CloseAsync();
                            return true;
                        }
                        else
                        {
                            //failed login attempt + 1
                            string query1 = "Update Radio.Account SET attempt = @attempt WHERE username = @username and id = @id";
                            using (MySqlCommand cmd1 = new MySqlCommand(query1, conn))
                            {
                                cmd1.Parameters.AddWithValue("@username", account.Username);
                                cmd1.Parameters.AddWithValue("@attempt", attempt + 1);
                                cmd1.Parameters.AddWithValue("@id", id);
                                await cmd1.ExecuteNonQueryAsync();

                            }

                        }
                    }
                    else
                    {
                        //Attempted more than 3 times Freeze account
                        query = "Update Radio.Account SET enabled = @enabled WHERE username = @username and id = @id";
                        using (MySqlCommand cmd1 = new MySqlCommand(query, conn))
                        {
                            cmd1.Parameters.AddWithValue("@username", account.Username);
                            cmd1.Parameters.AddWithValue("@enabled", 0);
                            cmd1.Parameters.AddWithValue("@id", id);
                            await cmd1.ExecuteNonQueryAsync();


                        }

                    }



                    await conn.CloseAsync();

                    return false;

                }
                catch (Exception e)
                {
                    string d = e.Message;
                    await conn.CloseAsync();
                    return false;
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

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", account.Username);
                        MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                if (reader.GetInt16(0) != 0)
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
                    query = "INSERT INTO Radio.Account (email,username,password,enabled,salt) VALUES (@email,@username,@password,@enabled,@salt)";
                    //Create User
                    string salt = Hash.CreateSalt(8);
                    account.Password = Hash.CreateHash(account.Password, salt);
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", account.Email);
                        cmd.Parameters.AddWithValue("@username", account.Username);
                        cmd.Parameters.AddWithValue("@password", account.Password);
                        cmd.Parameters.AddWithValue("@enabled", 1);
                        cmd.Parameters.AddWithValue("@salt", salt);

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
