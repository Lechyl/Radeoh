using WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using WebAPI.Helper;

namespace WebAPI.DAL
{
    public class MySqlDatabase : IDatabase
    {
        private string constring = "Data Source=radiodb-instance-1.cn2dn4x7sadv.us-east-1.rds.amazonaws.com; Initial Catalog =Radio; User ID =admin; Password =Bu$F0rrud3";
        /*
        public async Task<bool> BulkSaveFavorites(Account account)
        {
            //Get favorite from sqlite

            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                conn.Open();

                bool favoriteExist = false;
                string query = "select * from Radio.Favorite where slug = @slug";

                foreach (var item in sqlFavorites)
                {

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@slug", item.Slug);

                        MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                        favoriteExist = reader.HasRows;
                        reader.Close();
                    }

                    if (!favoriteExist)
                    {

                        string query1 = "INSERT INTO Radio.Favorite(slug,Title,country,lang,image,stream_url)VALUES(@slug,@title,@country,@lang,@image,@stream_url)";

                        using (MySqlCommand cmd = new MySqlCommand(query1, conn))
                        {
                            cmd.Parameters.AddWithValue("@slug", item.Slug);
                            cmd.Parameters.AddWithValue("@Title", item.Title);
                            cmd.Parameters.AddWithValue("@country", item.Country);
                            cmd.Parameters.AddWithValue("@lang", item.Lang);
                            cmd.Parameters.AddWithValue("@image", item.Image);
                            cmd.Parameters.AddWithValue("@stream_url", item.StreamUrl);

                            try
                            {
                                await cmd.ExecuteNonQueryAsync();
                            }
                            catch (Exception)
                            {
                                await conn.CloseAsync();
                                return false;
                            }
                        }
                    }
                }

                var query2 = new StringBuilder("INSERT INTO Radio.Account_has_favorites(idAccount,slugFavorite)VALUES");
                string id = Application.Current.Properties["tmpID"].ToString();
                List<string> rows = new List<string>();
                foreach (var item in sqlFavorites)
                {
                    rows.Add(string.Format("({0},'{1}')", int.Parse(id), MySqlHelper.EscapeString(item.Slug)));
                }
                query2.Append(string.Join(",", rows));

                using (MySqlCommand cmd = new MySqlCommand(query2.ToString(), conn))
                {
                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        await conn.CloseAsync();
                        return false;
                    }
                }
                await conn.CloseAsync();

                return true;

            }
        }
        */

        public async Task<Account> GetByID(int id)
        {
            Account account = new Account();
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                conn.Open();

                string query = "SELECT email,username FROM Radio.Account where id = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        account.Email = reader.GetString(0);
                        account.Username = reader.GetString(1);

                    }
                    reader.Close();

                }
                await conn.CloseAsync();
                return account;
            }
        }
        public async Task<bool> DeleteFavorite(int id,Favorite station)
        {
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                conn.Open();

                string query = "DELETE FROM Radio.Account_has_favorites WHERE slugFavorite = @slug AND idAccount = @id";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@slug", station.Slug);
                    cmd.Parameters.AddWithValue("@id", id);


                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception)
                    {
                        await conn.CloseAsync();
                        return false;
                    }
                }

                await conn.CloseAsync();
                return true;
            }
        }

        public async Task<List<Favorite>> GetFavorites(int id)
        {
            List<Favorite> list = new List<Favorite>();

            try
            {

                    using (MySqlConnection conn = new MySqlConnection(constring))
                    {
                        conn.Open();

                        string query = "SELECT slugFavorite FROM Radio.Account_has_favorites where idAccount = @id";
                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);

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
                

                return list;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<DtoAccount> Login(DtoAccount account)
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
                int attempt = 0;
                int enabled = 0;
                string query = "select password,id,attempt,enabled from Account where username = @username";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", account.Username);
                    MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {

                        while (await reader.ReadAsync())
                        {
                            hashedPassword = reader.GetString(0);
                            id = reader.GetInt32(1);
                            attempt = reader.GetInt32(2);
                            enabled = reader.GetInt16(3);
                        }
                    }
                    reader.Close();
                }


                try
                {

                    //Check if user attempted to login more than 3 times
                    if (attempt < 3 && enabled == 1)
                    {

                        if (Hash.VerifyHash(account.Password, hashedPassword))
                        {
                            //Logged in
                            //reset attempt to zero
                            string query1 = "Update Radio.Account SET attempt = @attempt WHERE username = @username and id = @id";
                            using (MySqlCommand cmd1 = new MySqlCommand(query1, conn))
                            {
                                cmd1.Parameters.AddWithValue("@username", account.Username);
                                cmd1.Parameters.AddWithValue("@attempt", 0);
                                cmd1.Parameters.AddWithValue("@id", id);
                                await cmd1.ExecuteNonQueryAsync();

                            }
                            account.Id = id;
                            await conn.CloseAsync();
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
                            return null;

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
                        return null;

                    }



                    await conn.CloseAsync();

                    return account;

                }
                catch (Exception e)
                {
                    string d = e.Message;
                    await conn.CloseAsync();
                    return null;
                }


            }
        }

        public async Task<DtoAccount> Register(Account account)
        {


                DtoAccount dtoAccount = new DtoAccount()
                {
                    Id = 0,
                    Email = account.Email,
                    Username = account.Username,
                    Password = account.Password
                };
                //Check if the Username exist in the database #Yes go on #No return and send back false boolean
                using (MySqlConnection conn = new MySqlConnection(constring))
                {
                try
                {
                    conn.Open();
                    string query = "select COUNT(*) as accounts from Account where username = @username";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", account.Username);
                        MySqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (reader.HasRows)
                        {
                            await reader.ReadAsync();

                            if (reader.GetInt16(0) != 0)
                            {
                                reader.Close();
                                await conn.CloseAsync();

                                return null;
                            }


                        }
                        else
                        {
                            reader.Close();
                            await conn.CloseAsync();

                            return null;

                        }

                        reader.Close();

                    }
                    query = "INSERT INTO Radio.Account (email,username,password,enabled) VALUES (@email,@username,@password,@enabled)";
                    //Create User

                    account.Password = Hash.CreateHash(account.Password);
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", account.Email);
                        cmd.Parameters.AddWithValue("@username", account.Username);
                        cmd.Parameters.AddWithValue("@password", account.Password);
                        cmd.Parameters.AddWithValue("@enabled", 1);


                        cmd.ExecuteNonQuery();
                        dtoAccount.Id = (int)cmd.LastInsertedId;
                    }
                    

                    await conn.CloseAsync();
                    return dtoAccount;

                }
                catch (Exception)
                {
                    await conn.CloseAsync();

                    return null;

                }
            }


        }

        public async Task<bool> SaveFavorite(int id,Favorite station)
        {
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                conn.Open();

                bool favoriteExist = false;
                string query = "select * from Radio.Favorite where slug = @slug";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@slug", station.Slug);

                    MySqlDataReader reader = await cmd.ExecuteReaderAsync();

                    favoriteExist = reader.HasRows;
                    reader.Close();
                }

                if (!favoriteExist)
                {

                    string query1 = "INSERT INTO Radio.Favorite(slug,Title,country,lang,image,stream_url)VALUES(@slug,@title,@country,@lang,@image,@stream_url)";

                    using (MySqlCommand cmd = new MySqlCommand(query1, conn))
                    {
                        cmd.Parameters.AddWithValue("@slug", station.Slug);
                        cmd.Parameters.AddWithValue("@Title", station.Title);
                        cmd.Parameters.AddWithValue("@country", station.Country);
                        cmd.Parameters.AddWithValue("@lang", station.Lang);
                        cmd.Parameters.AddWithValue("@image", station.Image);
                        cmd.Parameters.AddWithValue("@stream_url", station.StreamUrl);
                        try
                        {
                            await cmd.ExecuteNonQueryAsync();
                        }
                        catch (Exception)
                        {
                            await conn.CloseAsync();
                            return false;
                        }
                    }
                }

                string query2 = "INSERT INTO Radio.Account_has_favorites(idAccount,slugFavorite)VALUES(@id,@slug)";
                using (MySqlCommand cmd = new MySqlCommand(query2, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@slug", station.Slug);

                    await cmd.ExecuteNonQueryAsync();
                }
                await conn.CloseAsync();

                return true;

            }

        }
    }
}
