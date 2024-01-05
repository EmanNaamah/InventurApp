using InventurApp.Culture;
using InventurApp.Models;
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
    class NewCountingViewModel : INotifyPropertyChanged
    {
        public NewCountingViewModel()
        {
            ClientService = new ClientService();
            LoadInventoryIds().GetAwaiter();
            SaveLoginDataCommand = new Command(async x => await SaveLoginDataAsync());
            SaveNewStorageNr= new Command(async x => await SaveNewStoragDataAsync());
        }
        public ClientService ClientService { get; set; }
        public ICommand SaveNewStorageNr { get; set; }
        public ICommand SaveLoginDataCommand { get; set; }
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
        private string _storageCode { get; set; }
        public string StorageCode
        {
            get => _storageCode;
            set
            {
                _storageCode = value;
                NotifyPropertyChanged("StorageCode");
            }
        }
        private string _userNumber { get; set; }
        public string UserNumber
        {
            get => _userNumber;
            set
            {
                _userNumber = value;
                NotifyPropertyChanged("UserNumber");
            }
        }
        private List<int> _inventuryNumberList { get; set; }
        public List<int> InventuryNumberList
        {
            get => _inventuryNumberList;
            set
            {
                _inventuryNumberList = value;
                NotifyPropertyChanged("InventuryNumberList");
            }
        }
        private int _selectedInventuryNumber { get; set; }
        public int SelectedInventuryNumber
        {
            get => _selectedInventuryNumber;
            set
            {
                _selectedInventuryNumber = value;
                NotifyPropertyChanged("SelectedInventuryNumber");
            }
        }
        private async Task SaveLoginDataAsync()
        {
            if (string.IsNullOrEmpty(StorageCode) || string.IsNullOrEmpty(UserNumber))
            {
                return;
            }
            if (isValidLager())
            {
                var loginDataSettings = new LoginSettings()
                {
                    StorageId = StorageCode,
                    UserNumber = UserNumber
                };
                Preferences.Set(Statics.LoginSettings, JsonConvert.SerializeObject(loginDataSettings));

                await Shell.Current.GoToAsync("ContinueCountingPage");
            }
            else PopupMessage.ErrorPop(AppResources.ResourceManager.GetString("not_valid_StoregId")).GetAwaiter(); 
            
        }
        private async Task SaveNewStoragDataAsync()
        {
           var LoginSettings = JsonConvert.DeserializeObject<LoginSettings>(Preferences.Get(Statics.LoginSettings, JsonConvert.SerializeObject(new LoginSettings())));

            if (string.IsNullOrEmpty(StorageCode))
            {
                return;
            }
            if (isValidLager())
            {
                var loginDataSettings = new LoginSettings()
                {
                    //InventoryId = SelectedInventuryNumber,
                    StorageId = StorageCode,
                    UserNumber = LoginSettings.UserNumber
                };
                Preferences.Set(Statics.LoginSettings, JsonConvert.SerializeObject(loginDataSettings));

                await Shell.Current.GoToAsync("ContinueCountingPage");
            }
            else PopupMessage.ErrorPop(AppResources.ResourceManager.GetString("not_valid_StoregId")).GetAwaiter();

        }
        private bool isValidLager()
        {
            bool result = false;
            string code = StorageCode;
            string strLagerGruppe = "", strLagerort = "", strMagazin = "";

            string[] storage = code.Split('#');

            if (storage.Length < 2)
            {
                storage = code.Split('.');
            }
            if (storage.Length >= 2)
            {
                strLagerGruppe = storage[0];
                strLagerort = storage[1];

                if (storage.Length == 3) strMagazin = storage[2];

                result = !(strLagerGruppe.Length > 4 ||
                    strLagerort.Length > 10 ||
                    strMagazin.Length > 20);
            }
            return result;
        }
        private async Task LoadInventoryIds()
        {

            var inventoryData = await ClientService.GetInventoryData();
            if (inventoryData?.Data != null && inventoryData?.Data.Count != 0)
            {
                var inventoryIds = inventoryData.Data.Select(x => x.InventurKopfID).ToList();
                InventuryNumberList = inventoryIds;
                IsInventuryNumberListEnabled = true;

            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
