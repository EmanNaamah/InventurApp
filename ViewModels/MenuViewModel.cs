using InventurApp.Culture;
using InventurApp.Models;
using InventurApp.Models.UiModels;
using InventurApp.Services;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InventurApp.ViewModels
{
    public class MenuViewModel : ContentPage, INotifyPropertyChanged
    {
        
        public MenuViewModel()
        {
            OpenNewCounting = new Command(async () => await NewCounting());
            OpenContinueCounting = new Command(async () => await Shell.Current.GoToAsync("NewCountingPage"));
            OpenArtikellist = new Command(async () => await Shell.Current.GoToAsync("ArtikellistPage"));
            OpenDownloadItemlist = new Command(async () => await Shell.Current.GoToAsync("DownloadItemPage"));
            OpenImportItemlistOffline = new Command(async () => await Shell.Current.GoToAsync("ImportDBOfflinePage"));
            OpenUploadCounting = new Command(async () => await Upload());
            OpenExportCountingOffline = new Command(async () => await Shell.Current.GoToAsync("ExportCountingOfflinePage"));
            OpenCountedItemList = new Command(async () => await Shell.Current.GoToAsync("CountedItemListPage"));
            TestWebApiConnection = new Command(async () => await Test_WEbApi_Connection());
            
        }
        public ICommand OpenNewCounting { get; }
        public ICommand OpenContinueCounting { get; }
        public ICommand OpenInventurlist { get; }
        public ICommand OpenArtikellist { get; }
        public ICommand OpenDownloadItemlist { get; }
        public ICommand OpenImportItemlistOffline { get; }
        public ICommand OpenUploadCounting { get; }
        public ICommand OpenExportCountingOffline { get; }
        public ICommand TestWebApiConnection { get; set; }
        private bool _iswebApiCorrect { get; set; }
        public ICommand OpenCountedItemList { get; }
        public bool IsWebApiCorrect
        {
            get => _iswebApiCorrect;
            set
            {
                _iswebApiCorrect = value;
                NotifyPropertyChanged("IsWebApiCorrect");
            }
        }
        public ClientService ClientService { get; set; }
        public async Task Test_WEbApi_Connection()
        {
            try
            {
                ClientService = new ClientService();
                var goodsResult = await ClientService.DownloadArticles();
                if (goodsResult != null)
                { 
                    IsWebApiCorrect = true;
                }
                else
                {
                    IsWebApiCorrect = false;
                }
            }
            catch (System.Exception ex)
            {
                IsWebApiCorrect = false;
                //PopupMessage.ErrorPop(ex.Message.ToString()).GetAwaiter();
            }
        }
        private async Task NewCounting()
        {
            bool answer = await App.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("delete_counting_dialog_title"), AppResources.ResourceManager.GetString("Ask_delete_StoragList"), AppResources.ResourceManager.GetString("yes"), AppResources.ResourceManager.GetString("no"));
            if (answer)
            {
                try
                {

                    var isnull = await App.CountedItemsRepository.DeleteAll();
                    Preferences.Set(Statics.LoginSettings, JsonConvert.SerializeObject(new LoginSettings()));
                    if (isnull)
                    {
                        await Shell.Current.GoToAsync("NewCountingPage");
                    }

                }
                catch (System.Exception ex)
                {

                    await Application.Current.MainPage.DisplayAlert("Error",ex.Message , "OK"); 
                }
               
            }
        }
        private async Task Upload()
        {
            var Ask = await Application.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("Atert"), AppResources.ResourceManager.GetString("ask_ upload_Aktuel_StoragList"), AppResources.ResourceManager.GetString("yes"), AppResources.ResourceManager.GetString("no"));
            if (Ask)
            {
                await Shell.Current.GoToAsync("UploadCountingPage");
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
