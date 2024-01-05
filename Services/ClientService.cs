using InventurApp.Models;
using InventurApp.Models.ExportModels;
using InventurApp.Models.ImportModels;
using InventurApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InventurApp.Services
{
    public class ClientService
    {

        public ClientService()
        {
        settings = JsonConvert.DeserializeObject<Settings>(Preferences.Get(Statics.SettingsKey, JsonConvert.SerializeObject(new Settings())));
        }
        public string AppUrl { get; set; } = "https://erp4allthailand.ddns.net:7779/api";

        public Settings settings { get; set; }
        private HttpClient HttpClient { get; set; }
        #region ImportArtikel
        public async Task<ArticleData> DownloadArticles(string filter = "" )
        {
            try
            {
                string Url = $"{settings.ServerUrl}:{settings.Port}/api/data/request?viewName=InventurArtikel&filter={filter}";
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Add(settings.APIKey, settings.APIKeyPassword);
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(settings.APIKey, settings.APIKeyPassword);
                HttpResponseMessage response = await HttpClient.GetAsync(new Uri(Url));
                var textResponse = await response.Content.ReadAsStringAsync();
                var items = await response.Content.ReadFromJsonAsync<ArticleData>();
                return response.IsSuccessStatusCode ? items : new ArticleData();
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public async Task<InventoryData> GetInventoryData()
        {
            try
            {
                string Url = $"{settings.ServerUrl}:{settings.Port}/api/data/request?viewName=InventurKopf&filter=";
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Add(settings.APIKey, settings.APIKeyPassword);
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(settings.APIKey, settings.APIKeyPassword);
                HttpResponseMessage response = await HttpClient.GetAsync(new Uri(Url));
                var textResponse = await response.Content.ReadAsStringAsync();
                var inventoryData = await response.Content.ReadFromJsonAsync<InventoryData>();
                return response.IsSuccessStatusCode ? inventoryData : new InventoryData();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return null;
            }
        }
        #endregion ImportArtikel
        #region UploadArtikel
        public async Task<UploadRespons> UploadItems(UploadArtikels uploadArtikels)
        {
            try
            {
                var uploadBody = new UploadArtikels() { StocktakingID = uploadArtikels.StocktakingID, Stocktakings = uploadArtikels.Stocktakings };
                string Url = $"{settings.ServerUrl}:{settings.Port}/api/warehouse/countinventory";
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Add(settings.APIKey, settings.APIKeyPassword);
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(settings.APIKey, settings.APIKeyPassword);
                HttpResponseMessage response = await HttpClient.PostAsJsonAsync(new Uri(Url), uploadBody);
                var textResponse = await response.Content.ReadAsStringAsync();
                var items = await response.Content.ReadFromJsonAsync<UploadRespons>();
                return response.IsSuccessStatusCode ? items : new UploadRespons();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return null;
            }

        }


        #endregion UploadArtikel
    }
}
