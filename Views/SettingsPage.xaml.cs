using InventurApp.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventurApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }
        protected override void OnAppearing()
        {
            (this.BindingContext as SettingsViewModel).DisableEdit();
        }
        protected override void OnDisappearing()
        {
            (this.BindingContext as SettingsViewModel).DisableEdit();
        }

        protected override bool OnBackButtonPressed()
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
            });

            return true;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if ((this.BindingContext as SettingsViewModel).Settings.LogoutSeconds < 20)
            {
               
            }
            else
            {
         
                (this.BindingContext as SettingsViewModel).SaveCommand.Execute("");

            }
        }
       
    }
}