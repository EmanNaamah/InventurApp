using InventurApp.Culture;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventurApp.Models.UiModels
{
   public  class PopupMessage
    {
        public static async Task  ErrorPop(string massagetotranslate)
        {
            await App.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("Error"), massagetotranslate, "OK"); 
        }
        public static async Task AlertPop(string massagetotranslate)
        {
            await App.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("Alert"), AppResources.ResourceManager.GetString(massagetotranslate), "OK");
           
        }
        public static async Task savePop(string massagetotranslate)
        {
            await App.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("save"), massagetotranslate, "OK");

        }
        public static async Task InfoPop(string massagetotranslate)
        {
            await App.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("info"),massagetotranslate, "OK");
        }
        public static async Task<string> TexteditPop(string Titel,string Askotranslate)
        {
            string result = await App.Current.MainPage.DisplayPromptAsync(Titel, Askotranslate);
            return result;
        }
    }
}
