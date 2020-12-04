using Newtonsoft.Json;
using RadioApp.Converter;
using RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RadioApp.Services
{
    public class API : IAPIManager
    {
        private HttpClient client;

        private string databaseAPIIP = "10.142.65.152:5001";
        public API()
        {
            //Self signed Certificate
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            client = new HttpClient(clientHandler);
            //Ignore bad certificate in .NET core 2.0
            // System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));



        }

        public Task<bool> BulkSaveFavorites(Account account)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteFavorite( RadioStation station)
        {
            if (!Application.Current.Properties.ContainsKey("key"))
            {
                return false;
            }
            var favorite = Mapper.Map(station);

            string apiUrl = string.Format("https://{0}/api/favorite/{1}",databaseAPIIP,Application.Current.Properties["key"]);
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = uri,
                Content = new StringContent(JsonConvert.SerializeObject(favorite), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var success = JsonConvert.DeserializeObject<bool>(content);

                return success;
            }
            return false;
        }

        public async Task<List<Favorite>> GetFavorites()
        {
            List<Favorite> list = new List<Favorite>();
            if (!Application.Current.Properties.ContainsKey("key"))
            {
                return list;
            }

            string apiUrl = string.Format("https://{0}/api/favorite/{1}/all", databaseAPIIP,Application.Current.Properties["key"]);
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));

            var response = await client.GetAsync(uri).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<Favorite>>(content);

                return list;
            }
            return list;
        }

        //Get Radiostations from Extern API
        public async Task<List<RadioStation>> GetRadioStations()
        {

            string apiUrl = "https://radeoh.app/api/stations/";
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var RadioStations = JsonConvert.DeserializeObject<List<RadioStation>>(content);

                return RadioStations;
            }
            return null;
        }




        public async void GetRadioStationsTest()
        {
            string apiUrl = "https://radeoh.app/api/stations";
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                // var RadioStations = JsonConvert.DeserializeObject<List<RadioStation>>(content);
                Console.WriteLine(content);

            }

        }

        public async Task<DtoAccount> Login(Account account)
        {
            account.Email = "";
            string apiUrl = string.Format("https://{0}/api/account/login",databaseAPIIP);
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));
            var jsonData = JsonConvert.SerializeObject(account);
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri,
                Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                DtoAccount dtoAccount = JsonConvert.DeserializeObject<DtoAccount>(content);

                return dtoAccount;
            }
            return null;
        }

        public async Task<DtoAccount> Register(Account account)
        {
            string apiUrl = string.Format("https://{0}/api/account/register",databaseAPIIP);
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                DtoAccount dtoAccount = JsonConvert.DeserializeObject<DtoAccount>(content);

                return dtoAccount;
            }
            return null;
        }

        public async Task<bool> SaveFavorite(RadioStation station)
        {
            if(!Application.Current.Properties.ContainsKey("key"))
            {
                return false;
            }
            var favorite = Mapper.Map(station);

            string apiUrl = string.Format("https://{}/api/favorite/{1}",databaseAPIIP, Application.Current.Properties["key"]);
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = new StringContent(JsonConvert.SerializeObject(favorite), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var success = JsonConvert.DeserializeObject<bool>(content);

                return success;
            }
            return false;
        }
    }
}
