using Newtonsoft.Json;
using RadioApp.Converter;
using RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RadioApp.Services
{
    public class API : IAPIManager
    {
        private HttpClient client;

        public API()
        { 
             client = new HttpClient();
            //Ignore bad certificate in .NET core 2.0
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };



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

            string apiUrl = string.Format("https://localhost:44369/api/favorite/{0}", Application.Current.Properties["key"]);
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

            string apiUrl = string.Format("https://localhost:44369/api/favorite/{0}/all", Application.Current.Properties["key"]);
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
            string apiUrl = "http://localhost:5000/api/account/login";
            Uri uri = new Uri(string.Format(apiUrl, string.Empty));
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
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

        public async Task<DtoAccount> Register(Account account)
        {
            string apiUrl = "https://localhost:44369/api/account/register";
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

            string apiUrl = string.Format("https://localhost:44369/api/favorite/{0}", Application.Current.Properties["key"]);
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
