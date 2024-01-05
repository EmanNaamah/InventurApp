using InventurApp.Interfaces;
using InventurApp.ViewModels;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventurApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSerialNummerPage : ContentPage
    {
        public AddSerialNummerPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            await Task.Delay(100);
            TxtBarcode.Focus();
        }

        private void OnDelete(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            var itemToDelete = item.CommandParameter.ToString();
            (this.BindingContext as ContinueCountingViewModel).DeleteserialNummer(itemToDelete);

        }
        async void BtnScan_Clicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    TxtBarcode.Text = result;
                    (this.BindingContext as ContinueCountingViewModel).AddserialNr.Execute("");
                    TxtBarcode.Text = "";
                    await Task.Delay(100);
                    TxtBarcode.Focus();
                }
                else await Task.Delay(100);
                TxtBarcode.Focus();
            }
            catch (Exception)
            {
                throw;
            }
        }
        async void BtnSearch_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(TxtBarcode.Text))
                {
                    (this.BindingContext as ContinueCountingViewModel).AddserialNr.Execute("");
                }
                await Task.Delay(100);
                TxtBarcode.Focus();
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected override bool OnBackButtonPressed()
        {
            (this.BindingContext as ContinueCountingViewModel).TestSiriNrCountbeforBack.Execute("");
            return false;
        }

        //private void btnSave_Clicked(object sender, EventArgs e)
        //{
        //    (this.BindingContext as ContinueCountingViewModel).TestSiriNrCountbeforBack.Execute("");
        //}
    }
}