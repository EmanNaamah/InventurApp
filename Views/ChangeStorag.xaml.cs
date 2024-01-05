using InventurApp.Interfaces;
using InventurApp.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventurApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeStorag : ContentPage
    {
        public ChangeStorag()
        {
            InitializeComponent();
        }
        async void BtnScan_Clicked(object sender, EventArgs e)
        {
            try
            {
                var scanner = DependencyService.Get<IQrScanningService>();
                var result = await scanner.ScanAsync();
                if (result != null)
                {
                    TxtStoregcode.Text = result;
                    (this.BindingContext as NewCountingViewModel).SaveNewStorageNr.Execute(null);
                }
                TxtStoregcode.Focus();
            }
            catch (Exception)
            {
                throw;
            }

        }
        private void TxtBarcode1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtStoregcode.Text))
                TxtStoregcode.Text = TxtStoregcode.Text.ToUpper();
        }

        private void save_New_StorageCode(object sender, EventArgs e)
        {
           if(!string.IsNullOrEmpty( TxtStoregcode.Text))
            (this.BindingContext as NewCountingViewModel).SaveNewStorageNr.Execute(null);
        }
        protected override async void OnAppearing()
        {
            await Task.Delay(100);
                    TxtStoregcode.Focus();
        }
    }
}