using InventurApp.Culture;
using InventurApp.Models;
using InventurApp.Models.ExportModels;
using InventurApp.Models.UiModels;
using InventurApp.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InventurApp.ViewModels
{
    public class UploadCountingViewModel : INotifyPropertyChanged
    {
        public UploadCountingViewModel()
        {
            ClientService = new ClientService();
            settings = JsonConvert.DeserializeObject<Settings>(Preferences.Get(Statics.SettingsKey, JsonConvert.SerializeObject(new Settings())));
            LoadInventoryIds().GetAwaiter();
            UploadInventurListCommand = new Command(async () => await UploadInventurList());
        }
        public Settings settings { get; set; }
        public ClientService ClientService { get; set; }
        private async Task LoadInventoryIds()
        {
            IsBusy = true;
             var inventoryData = await ClientService.GetInventoryData();

            if (inventoryData?.Data != null && inventoryData?.Data.Count != 0)
            {
                
                var inventoryInkBemerkungs = inventoryData.Data.Select(x => x.InkBemerkung).ToList();
                InventuryNumberList = inventoryInkBemerkungs;

                IsInventuryNumberListEnabled = true;
            }
            IsBusy = false;
        }
        private bool _isBusy { get; set; }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }
        private List<string> _inventuryNumberList { get; set; }
        public List<string> InventuryNumberList
        {
            get => _inventuryNumberList;
            set
            {
                _inventuryNumberList = value;
                NotifyPropertyChanged("InventuryNumberList");
            }
        }

        private bool _isInventuryNumberListEnabled { get; set; }
        public bool IsInventuryNumberListEnabled
        {
            get => _isInventuryNumberListEnabled;
            set
            {
                _isInventuryNumberListEnabled = value;
                NotifyPropertyChanged("IsInventuryNumberListEnabled");
            }
        }
        public ICommand UploadInventurListCommand { get; set; }
        private string _selectedInventuryNumber { get; set; }
        public string SelectedInventuryNumber
        {
            get => _selectedInventuryNumber;
            set
            {
                _selectedInventuryNumber = value;
                NotifyPropertyChanged("SelectedInventuryNumber");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task  UploadInventurList()
        {
            if(!string.IsNullOrEmpty(SelectedInventuryNumber))
            {
                var inventoryData = await ClientService.GetInventoryData();
                
                if (inventoryData?.Data != null && inventoryData?.Data.Count != 0)
                {
                    var InventurKopfIDs = inventoryData.Data.First(x => x.InkBemerkung == SelectedInventuryNumber);
                    SelectedInventuryNumber = InventurKopfIDs.InventurKopfID.ToString();
                    IsInventuryNumberListEnabled = true;
                }
                IsBusy = true;
                var items = await App.CountedItemsRepository.GetItems();
                var stocktakings = items.ToStocktakings();
                UploadArtikels uploadArtikels = new UploadArtikels()
                {
                    StocktakingID = SelectedInventuryNumber.ToString(),
                    Stocktakings = stocktakings,
                };
               var response= await ClientService.UploadItems(uploadArtikels);
                IsBusy = false;
                if (response.HttpStatusCode == 200)
                {
                    PopupMessage.InfoPop($"{AppResources.ResourceManager.GetString($"Inventoryread")}'{response.CountReaded}'").GetAwaiter();
                    await Shell.Current.GoToAsync("MenuPage");
                }

            }
            else
                PopupMessage.ErrorPop($"{AppResources.ResourceManager.GetString($"please_select_InventurId")}").GetAwaiter(); 
           
        }
    }

    #region ExtantionMethods
    public static class ExportArticleExtensions
    {
        public static List<Stocktaking> ToStocktakings(this List<ExportArticle> articles)
        {
            return articles.Select(x => x.ToStocktaking()).ToList();
        }

        private static Stocktaking ToStocktaking(this ExportArticle article)
        {
            var stocktaking = new Stocktaking()
            {
                ArticleNumber = article.ArticleNumber,
                Qty1 = article.Qtyunit1,
                Qty2 = article.Qtyunit2,
                Cf1 = article.Cf1,
                Cf2 = article.Cf2,
                Cf3 = article.Cf3,
                Cf4 = article.Cf4,
                serialNumber = article.Serialnumber,
                UserNumber=article.UserNumber,
                Lot= article.Charge,
                countryoforigin = article.KZLand,
                Date = article.Date,
                 
            };

            var storageProperties = article.Storage.Split('#');
            if (storageProperties.Length < 2)
            {
                storageProperties = article.Storage.Split('.');
            }
            if (storageProperties.Length > 1)
            {
                if (storageProperties.Length == 3)
                {
                    stocktaking.Storage1 = storageProperties[0];
                    stocktaking.Storage2 = storageProperties[1];
                    stocktaking.Storage3 = storageProperties[2];
                }
                if (storageProperties.Length == 2)
                {
                    stocktaking.Storage1 = storageProperties[0];
                    stocktaking.Storage2 = storageProperties[1];
                    stocktaking.Storage3 = "";
                }

            }
            return stocktaking;
        }
    }
    #endregion ExtantionMethods
}
