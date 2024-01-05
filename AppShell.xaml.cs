using InventurApp.Culture;
using InventurApp.ViewModels;
using InventurApp.Views;
using Newtonsoft.Json;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading;
using InventurApp.Models;

namespace InventurApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            DevExpress.XamarinForms.Navigation.Initializer.Init();
            DevExpress.XamarinForms.Editors.Initializer.Init();
            DevExpress.XamarinForms.DataGrid.Initializer.Init();
            InitializeComponent();
            Routing.RegisterRoute(nameof(MenuPage), typeof(MenuPage));
            Routing.RegisterRoute(nameof(NewCountingPage), typeof(NewCountingPage));
            Routing.RegisterRoute(nameof(ChangeStorag), typeof(ChangeStorag));
            Routing.RegisterRoute(nameof(ContinueCountingPage), typeof(ContinueCountingPage));
            Routing.RegisterRoute(nameof(DownloadItemPage), typeof(DownloadItemPage));
            Routing.RegisterRoute(nameof(CountedItemListPage), typeof(CountedItemListPage));
            Routing.RegisterRoute(nameof(ArtikellistPage), typeof(ArtikellistPage));
            Routing.RegisterRoute(nameof(ImportDBOfflinePage), typeof(ImportDBOfflinePage));
            Routing.RegisterRoute(nameof(ExportCountingOfflinePage), typeof(ExportCountingOfflinePage));
            Routing.RegisterRoute(nameof(UploadCountingPage), typeof(UploadCountingPage));
            Routing.RegisterRoute(nameof(AddSerialNummerPage), typeof(AddSerialNummerPage));

             var settings = JsonConvert.DeserializeObject<Settings>(Preferences.Get(Statics.SettingsKey, JsonConvert.SerializeObject(new Settings())));
            var tt = settings.Language.ToString();
            CultureInfo culture = new CultureInfo(tt);
            Thread.CurrentThread.CurrentUICulture = culture;
            AppResources.Culture = culture;
            Init();

        }
        public void Init()
        {
            mainbar.CurrentItem = main_tab_bar_dial;
        }

    }
   

}
