using System.Windows.Input;
using Xamarin.Forms;

namespace InventurApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            OpenMenuPage = new Command(async () => await Shell.Current.GoToAsync("MenuPage"));
        }
        public ICommand OpenMenuPage { get; }
    }
}
