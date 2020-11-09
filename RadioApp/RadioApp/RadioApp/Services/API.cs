using Newtonsoft.Json;
using RadioApp.DAL;
using RadioApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        }

        //Get Radiostations from Extern API
        public async Task<List<RadioStation>> GetRadioStations()
        {

            string apiUrl = "https://radeoh.app/api/stations";
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
    }
}
