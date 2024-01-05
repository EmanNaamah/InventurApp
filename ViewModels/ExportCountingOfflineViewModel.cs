using InventurApp.Culture;
using InventurApp.Models.UiModels;
using InventurApp.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InventurApp.ViewModels
{
    class ExportCountingOfflineViewModel
    {
        public ExportCountingOfflineViewModel()
        {
            importExportDBService = new ImportExportDBService();
            ExportToDatCommand = new Command(async () => await SaveDB3File());
            // ExportArtikelToDatCommand = new Command(async () => await SaveDB3ArtikelFile());
            ShareDatCommand = new Command(async () => await ShareFile());
        }
        public ImportExportDBService importExportDBService;
        public ICommand ExportToDatCommand { private set; get; }
        // public ICommand ExportArtikelToDatCommand { private set; get; }

        public ICommand ShareDatCommand { get; set; }
        public async Task SaveDB3File()
        {
            if (await PermissionUtils.GetPermission<Permissions.StorageWrite>())
            {
                await importExportDBService.SaveBD();
            }
            else PopupMessage.ErrorPop(AppResources.ResourceManager.GetString("dontpermission")).GetAwaiter();
        }
        public async Task ShareFile()
        {
            if (await PermissionUtils.GetPermission<Permissions.StorageRead>())
            {
                await importExportDBService.ShareDB();
            }
            else PopupMessage.ErrorPop(AppResources.ResourceManager.GetString("dontpermission")).GetAwaiter();
        }
    }
}

