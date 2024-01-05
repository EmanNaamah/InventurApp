using InventurApp.Culture;
using InventurApp.Models.UiModels;
using InventurApp.Services;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InventurApp.ViewModels
{
    class ImportDBOfflineViewModel : INotifyPropertyChanged
    {
        public ImportDBOfflineViewModel()
        {
            ImportExportDBService = new ImportExportDBService();
            //ImportDatCommand = new Command(async () => await PickAndShow());
            ImportArtikelDatCommand = new Command(async () => await PickAndShowForArtikel());
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
        public ImportExportDBService ImportExportDBService { get; }
        public ICommand ImportArtikelDatCommand { private set; get; }
        // public ICommand ImportDatCommand { private set; get; }
        //public async Task PickAndShow()
        //{
        //    if (await PermissionUtils.GetPermission<Permissions.StorageRead>())
        //    {
        //        IsBusy = true;
        //        await ImportExportDBService.ImportDB();
        //        IsBusy = false;
        //    }

        //    else
        //    {
        //        IsBusy = false;
        //       PopupMessage.ErrorPop("dontpermission").GetAwaiter();

        //    }


        //}
        public async Task PickAndShowForArtikel()
        {
            if (await PermissionUtils.GetPermission<Permissions.StorageRead>())
            {
                IsBusy = true;
                await ImportExportDBService.ImportArtikelDB();
                IsBusy = false;
            }

            else
            {
                IsBusy = false;
                PopupMessage.ErrorPop(AppResources.ResourceManager.GetString("dontpermission")).GetAwaiter();
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
