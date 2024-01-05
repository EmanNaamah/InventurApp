using InventurApp.Culture;
using Xamarin.Forms;

namespace InventurApp.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }
        protected override bool OnBackButtonPressed()
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert(AppResources.ResourceManager.GetString("Alert"), AppResources.ResourceManager.GetString("closeapp"), AppResources.ResourceManager.GetString("yes"), AppResources.ResourceManager.GetString("no")))
                {
                    System.Environment.Exit(0);
                }



            });

            return true;
        }
    }
}
