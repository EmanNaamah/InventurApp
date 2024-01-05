
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InventurApp.ViewModels
{
    public class InfoViewModel : BaseViewModel
    {
        public InfoViewModel()
        {
            Title = "Info";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://erp4all.de/"));
        }
        public ICommand OpenWebCommand { get; }
    }
}
