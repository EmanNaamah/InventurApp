using InventurApp.Interfaces;
using InventurApp.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventurApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCountingPage : ContentPage
    {
        public NewCountingPage()
        {
            InitializeComponent();
        }
       
        protected override async void OnAppearing()
        {
           
            await Task.Delay(100);
            TxtUseNumber.Focus();
      
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
                    (this.BindingContext as NewCountingViewModel).SaveLoginDataCommand.Execute(null);
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
        private void TxtStoregcode_Completed(object sender, EventArgs e)
        {
            (this.BindingContext as NewCountingViewModel).SaveLoginDataCommand.Execute(null);
            
        }
        private async void TxtUseNumber_CompletedAsync(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtUseNumber.Text))
            {
                await Task.Delay(200);
                TxtStoregcode.Focus();
            }
        }
    }
}