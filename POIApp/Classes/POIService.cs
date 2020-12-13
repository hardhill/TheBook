using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace POIApp.Classes
{
    class POIService
    {
        private const string GET_POIS = "https://private-e451d-poilist.apiary-mock.com/com.packt.poiapp/api/poi/pois";
        private const string CREATE_POI = "https://private-e451d-poilist.apiary-mock.com/com.packt.poiapp/api/poi/create";
        private const string DELETE_POI = "https://private-e451d-poilist.apiary-mock.com/com.packt.poiapp/api/poi/delete";

        public async Task<List<PointOfInterest>> GetPOIListAsync()
        {
            List<PointOfInterest> poiListData = null;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await httpClient.GetAsync(GET_POIS);
            if (response != null || response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                poiListData = new List<PointOfInterest>();
                JObject jsonResponse = JObject.Parse(content);
                IList<JToken> results = jsonResponse["pois"].ToList();
                foreach(JToken token in results)
                {
                    PointOfInterest poi = token.ToObject<PointOfInterest>();
                    poiListData.Add(poi);
                }
                return poiListData;
            }
            else
            {
                
                return null;
            }

        }

        public bool isConnected(Context activity)
        {
            var connectivityManager = (ConnectivityManager)activity.
           GetSystemService(Context.ConnectivityService);
            var activeConnection = connectivityManager.ActiveNetworkInfo;
            return (null != activeConnection && activeConnection.IsConnected);
        }

        public async Task<String> CreateOrUpdatePOIAsync(PointOfInterest poi)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new POIContractResolver();
            var poiJson = JsonConvert.SerializeObject(poi, Formatting.Indented, settings);
            HttpClient httpClient = new HttpClient();
            StringContent jsonContent = new StringContent(poiJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(CREATE_POI, jsonContent);
            if (response != null || response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }

            return null;

        }

        public async Task<String> DeletePOIAsync(int poiId)
        {
            HttpClient httpClient = new HttpClient();
            String url = String.Format(DELETE_POI, poiId);
            HttpResponseMessage response = await httpClient.DeleteAsync(url);
            if (response != null || response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            return null;
        }

        private class POIContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string key)
            {
                return key.ToLower();
            }
        }


    }

    
}