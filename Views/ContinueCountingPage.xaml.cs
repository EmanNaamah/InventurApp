using DevExpress.XamarinForms.Editors;
using DocumentFormat.OpenXml.Office2013.PowerPoint.Roaming;
using InventurApp.Culture;
using InventurApp.Interfaces;
using InventurApp.Models.UiModels;
using InventurApp.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Views.View;
using static InventurApp.ViewModels.ContinueCountingViewModel;

namespace InventurApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContinueCountingPage : ContentPage
    {
        public ContinueCountingPage()
        {
            Initializer.Init();
            InitializeComponent();
        }
        async  void BtnScannNewItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result =await GetScanCode();
                if (!string.IsNullOrEmpty(result))
                {
                    TxtBarcode1.Text = result;
                    await SearchItem();
                    await ChangeFoucous();
                }
                else
                    await ChangeFoucous();
            }
            catch (Exception ex)
            {
                    PopupMessage.ErrorPop(ex.Message).GetAwaiter();
            }
        }
        private async Task SearchItem()
        {
            AmountNumeric.IsEnabled = true;
            (this.BindingContext as ContinueCountingViewModel).SearchArticleCommand.Execute("");
            await Task.Delay(100);
            if (string.IsNullOrEmpty(TxtBarcode1.Text))
                TxtBarcode1.IsEnabled = true;
            else TxtBarcode1.IsEnabled = false;
            await Task.Delay(100);
            if ((this.BindingContext as ContinueCountingViewModel).ArticleModel.AddSerButtonEnabled)
            {

                AmountNumeric2.IsEnabled = false;
                AmountNumeric2.Value = 0;
            }
            else AmountNumeric2.IsEnabled = true;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await ChangeFoucous();
            });
        }
         async Task<String> GetScanCode()
        {
            var scanner = DependencyService.Get<IQrScanningService>();
            var result = await scanner.ScanAsync();
            return result;
        } 
        async void BtnScanCharge_Clicked(object sender, EventArgs e)
        {
            try
            {
                var result =await GetScanCode();
                if (!string.IsNullOrEmpty(result))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Task.Delay(200);
                        Charge.Text = result;
                        await ChangeFoucous ();
                    });
                   
                }else
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await ChangeFoucous();
                    });

            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    PopupMessage.ErrorPop(ex.Message).GetAwaiter();
                });
            }
        }
        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var answer = await Application.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("Alert"), AppResources.ResourceManager.GetString("close"), AppResources.ResourceManager.GetString("yes"), AppResources.ResourceManager.GetString("no"));
                if (answer)
                {
                    await Shell.Current.GoToAsync("MenuPage");
                }
            });
            return true;
        }
        protected override async void OnAppearing()
        {
         
              
                await Task.Delay(100);
              //  (this.BindingContext as ContinueCountingViewModel).IsLagerCodeAvailabel.Execute("");
                if ((this.BindingContext as ContinueCountingViewModel).ArticleModel.AddSerButtonEnabled)
                {
                    TxtBarcode1.IsEnabled = false;
                    AmountNumeric2.IsEnabled = false;
                    if (MyStorage.SerialNumbersList.Count != 0) AmountNumeric.IsEnabled = false;
                    (this.BindingContext as ContinueCountingViewModel).ArticleModel.Measuringunit1 = MyStorage.SerialNumbersList.Count;
                    var clone = (this.BindingContext as ContinueCountingViewModel).ArticleModel;
                    (this.BindingContext as ContinueCountingViewModel).ArticleModel = new Models.ArticleModel();
                    (this.BindingContext as ContinueCountingViewModel).ArticleModel = clone;
                }
                else
                {
                    await Task.Delay(100);
                    TxtBarcode1.IsEnabled = true;
                    TxtBarcode1.Focus();
                    AmountNumeric.IsEnabled = false;
                }
           
        }
      
        public bool TestIfAllFieldsFill()
        {
            var cansave = false;
            if(Charge.IsVisible && !String.IsNullOrEmpty(Charge.Text))
            cansave = true;
            else if(Charge.IsVisible && String.IsNullOrEmpty(Charge.Text))
            cansave = false;
            if (Kzland.IsVisible && !string.IsNullOrEmpty(Kzland.Text))
                cansave = true;
            else if(Kzland.IsVisible && string.IsNullOrEmpty(Kzland.Text))
                cansave = false;
            if (!AmountNumeric2.IsVisible && AmountNumeric.Value > 0)
                cansave = true;
            if (!AmountNumeric2.IsVisible && AmountNumeric.Value <= 0 )
                cansave = false;
            if (AmountNumeric2.IsVisible && AmountNumeric2.Value > 0 || AmountNumeric.Value > 0)
                cansave = true;
            if(AmountNumeric2.IsVisible && AmountNumeric.Value <= 0 && AmountNumeric2.Value <= 0)
                cansave = false;
            if(SizeText1.IsVisible && SizeText1.Value >0)
                cansave = true;
            else if (SizeText1.IsVisible && SizeText1.Value <=0)
                cansave = false;
            if(SizeText2.IsVisible && SizeText2.Value > 0)
                cansave = true;
            else if (SizeText2.IsVisible && SizeText2.Value <= 0)
                cansave = false;
            if(SizeText3.IsVisible && SizeText3.Value >0)
               cansave = true;
            else if(SizeText3.IsVisible && SizeText3.Value <=0)
             cansave = false;
            if(SizeText4.IsVisible && SizeText4.Value >0)
              cansave = true;
            else if(SizeText4.IsVisible && SizeText4.Value <=0)
            cansave = false;
            return cansave;        
        }
        public async Task ChangeFoucous()
        {
            await Task.Delay(600);
            if (string.IsNullOrEmpty(TxtBarcode1.Text))
                TxtBarcode1.Focus();
            else if (String.IsNullOrEmpty(Charge.Text) && Charge.IsEnabled)
                Charge.Focus();
            else if (Kzland.IsEnabled && String.IsNullOrEmpty(Kzland.Text))
                Kzland.Focus();
            else if (AmountNumeric.Value <= 0 && !AmountNumeric2.IsEnabled)
                AmountNumeric.Focus();
            else if (AmountNumeric.Value <= 0 && AmountNumeric2.IsEnabled && AmountNumeric2.Value <= 0)
                AmountNumeric.Focus();
            else if (AmountNumeric2.IsVisible && AmountNumeric2.Value <= 0 && AmountNumeric.Value <= 0)
                AmountNumeric2.Focus();
            else if (SizeText1.IsVisible && SizeText1.Value <= 0)
                SizeText1.Focus();
            else if (SizeText2.IsVisible && SizeText2.Value <= 0)
                SizeText2.Focus();          
            else if (SizeText3.IsVisible && SizeText3.Value <= 0)
                SizeText3.Focus();          
            else if (SizeText4.IsVisible && SizeText4.Value <= 0)
                SizeText4.Focus();
        }
        private async void BtnSave_Pressed(object sender, EventArgs e)
        {
            await Task.Delay(100);
          await  save();
        }
        public async Task save()
        {
            var canExcute = true;
            bool Qu1andQu2 = AmountNumeric2.IsVisible;
            bool serNrvisibel = addseri.IsVisible;
            if (Qu1andQu2)
            {
                Qu1andQu2 = true;
                if (AmountNumeric.Value > 0 && AmountNumeric2.Value > 0)
                {
                    await DisplayAlert("Error", AppResources.ResourceManager.GetString("quantity_error_dialog_description"), "Ok");
                    canExcute = false;
                }
                else if (AmountNumeric.Value>0 || AmountNumeric2.Value > 0)
                {
                    canExcute = true;
                }
                else if (AmountNumeric.Value <=0 && AmountNumeric2.Value <=0)
                {
                    await DisplayAlert("Error", AppResources.ResourceManager.GetString("quantity_error_dialog_description"), "Ok");
                    canExcute = false;
                }
            }
            if(Kzland.IsVisible && string.IsNullOrEmpty(Kzland.Text))
            {
                canExcute = false;
            }
            if (string.IsNullOrEmpty(Charge.Text) && Charge.IsVisible)
            {
                canExcute = false;
            }
            if (!Qu1andQu2 && AmountNumeric.Value <=0)
            {
                canExcute = false;
            }
            if (SizeText1.Value <= 0 && SizeText1.IsVisible)
            {
                canExcute = false;
            }
            if (SizeText2.Value <= 0 && SizeText2.IsVisible)
            {
                canExcute = false;
            }
            if (SizeText3.Value <= 0 && SizeText3.IsVisible)
            {
                canExcute = false;

            }
            if (SizeText4.Value <= 0 && SizeText4.IsVisible)
            {
                canExcute = false;
            }
            if (AmountNumeric.Value <= 0 && serNrvisibel)
            {
                canExcute = true;
            }
            if (canExcute)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    (this.BindingContext as ContinueCountingViewModel).SaveCommand.Execute(null);
                    TxtBarcode1.IsEnabled = true;
                    TxtBarcode1.Focus();
                    AmountNumeric.IsEnabled = false;
                });
               
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    if (!Qu1andQu2)
                        await DisplayAlert("Error", AppResources.ResourceManager.GetString("CheckValue"), "Ok");
                });
            }
        }
        private void BtnNewScan_Pressed(object sender, EventArgs e)
        {
            if ( AmountNumeric.Value <= 0 && AmountNumeric2.Value <= 0 && SizeText1.Value <= 0 && SizeText2.Value <= 0 && SizeText3.Value <= 0 && SizeText4.Value <= 0 && string.IsNullOrEmpty(Charge.Text))
            { (this.BindingContext as ContinueCountingViewModel).ResetCommand.Execute(null);
                TxtBarcode1.IsEnabled = true;
                ChangeFoucous();
            }
            else Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("Alert", AppResources.ResourceManager.GetString("delete_counting_dialog_description"), AppResources.ResourceManager.GetString("yes"), AppResources.ResourceManager.GetString("no")))
                {
                    (this.BindingContext as ContinueCountingViewModel).ResetCommand.Execute(null);
                    TxtBarcode1.IsEnabled = true;
                    ChangeFoucous();
                }
            });
            Device.BeginInvokeOnMainThread(async () =>
            {
                await ChangeFoucous();
            });
        }
        private void BtnAddSeri_Pressed(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("AddSerialNummerPage");
            }); 
        }
        protected void BtnChangeStoreg_Pressed(object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (await DisplayAlert("Alert", AppResources.ResourceManager.GetString("ask_leave_page_counting"), AppResources.ResourceManager.GetString("yes"), AppResources.ResourceManager.GetString("no")))
                {
                    await Shell.Current.GoToAsync("ChangeStorag");

                }
            });
        }
        //private async void NumricEntry_completed(object sender, EventArgs e)
        //{
        //    if (!TestIfAllFieldsFill())
        //    {
        //      Device.BeginInvokeOnMainThread(async () =>
        //    {
        //        await ChangeFoucous();
        //    });
        //    }
        //    else await save();
        //}
        private async void EntrySearch_Completed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtBarcode1.Text))
            {
               await SearchItem();
              
            }
        }
        private void TxtBarcode1_TextChanged(object sender, EventArgs e)
        {
                if (!string.IsNullOrWhiteSpace(TxtBarcode1.Text))
                TxtBarcode1.Text = TxtBarcode1.Text.ToUpper();
        }
        private void TxtKzLand_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Kzland.Text) && Kzland.Text.Length < 4)
                Kzland.Text = Kzland.Text.ToUpper();
            else if (Kzland.Text.Length > 3)
                Kzland.Text = Kzland.Text.Substring(0,3);
          
        }
        private void TxtCharge_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Charge.Text))
                Charge.Text = Charge.Text.ToUpper();
        }
        private async void TextEntry_completed(object sender, EventArgs e)
        {
            if (!TestIfAllFieldsFill())
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await ChangeFoucous();
                });
            }
            else await save();
        }


    }
}