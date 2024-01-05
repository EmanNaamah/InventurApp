using InventurApp.Culture;
using InventurApp.Models;
using InventurApp.Models.UiModels;
using InventurApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InventurApp.Services
{
    class ImportExportDBService
    {
        public ImportExportDBService()
        {
            Settings = JsonConvert.DeserializeObject<Settings>(Preferences.Get(Statics.SettingsKey, JsonConvert.SerializeObject(new Settings())));
        }
        public Settings Settings { get; set; }
        public string FileName { get; set; }
        public async Task SaveBD()
        {
                var time = DateTime.Now.ToString("yyyyMMddhhmmss");
                var DeviceId = Settings.DeviceId;
                FileName = $"INV{DeviceId}{time}.dat";
                var datService = new DatService();
                await datService.CreateDatFile($"/storage/emulated/0/Download/{FileName}");
          
          
        }
        public async Task ShareDB()
        {
            
               var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    FileName = $"File Name: {result.FileName}";
                    if (result.FileName.EndsWith("dat", StringComparison.OrdinalIgnoreCase))
                    {
                        await Share.RequestAsync(new ShareFileRequest
                        {
                            Title = "ShareFile.db3",
                            File = new ShareFile(result)
                        });
                    }
                   
                }
                else PopupMessage.ErrorPop(AppResources.ResourceManager.GetString("notDBfile")).GetAwaiter();
        }
        public async Task ImportArtikelDB()
        {
           
                var result = await FilePicker.PickAsync();
                if (result != null)
                {
                    var datService = new DatService();
                    await datService.ImportDatArtikelFile(result.FullPath);
                }
            
            
        }
    }
}
