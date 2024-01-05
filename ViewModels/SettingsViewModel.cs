using InventurApp.Culture;
using InventurApp.Models;
using InventurApp.Models.UiModels;
using InventurApp.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InventurApp.ViewModels
{
    public class SettingsViewModel : ContentPage, INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            try
            {
                SaveCommand = new Command(async () => await Save());
                RestoreCommand = new Command(async () => await Restore());
                EnableEditCommand = new Command(async () => await EnableEdit());
                Settings = GetSettingsFromLocalStorage() ?? new Settings();
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread( () =>
                {
                    PopupMessage.ErrorPop(e.Message).GetAwaiter();
                }); 
            }

        }
        public ICommand RestoreCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand EnableEditCommand { get; set; }

        private Settings _settings { get; set; }
        public Settings Settings
        {
            get => _settings;
            set
            {
                if (value != null)
                    _settings = value;

                NotifyPropertyChanged("Settings");
            }
        }
        private bool _isEditButtonEnabled { get; set; } = true;
        public bool IsEditButtonEnabled
        {
            get => _isEditButtonEnabled;
            set
            {
                _isEditButtonEnabled = value;
                NotifyPropertyChanged("IsEditButtonEnabled");
            }
        }
        private bool _isEnabled { get; set; }
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                NotifyPropertyChanged("IsEnabled");
            }
        }
        private async Task EnableEdit()
        {
            var enterpass = AppResources.ResourceManager.GetString("enterpass");
            var login = AppResources.ResourceManager.GetString("login");
            

            var result = "";
            while (result != Settings.UserPassword)
            {
                result =await App.Current.MainPage.DisplayPromptAsync(login, enterpass);
                if (result == Settings.UserPassword)
                {
                    IsEnabled = true;
                    IsEditButtonEnabled = false;
                    break;
                }
                if (result == null)
                    break;

                else
                {
                    IsEnabled = false;
                }
            }

        }
        internal void DisableEdit()
        {
            IsEnabled = false;
            IsEditButtonEnabled = true;
        }
        public List<Language> Languages { get; set; } = new List<Language>() { Language.EN, Language.DE };
        private Settings GetSettingsFromLocalStorage()
        {
            var settings = JsonConvert.DeserializeObject<Settings>(Preferences.Get(Statics.SettingsKey, JsonConvert.SerializeObject(new Settings())));
            return settings;
        }
        private async Task Restore()
        {
            var Restore = AppResources.ResourceManager.GetString("Restore");

            bool answer = await App.Current.MainPage.DisplayAlert(Restore, AppResources.ResourceManager.GetString("RestorAsk"), AppResources.ResourceManager.GetString("yes"), AppResources.ResourceManager.GetString("no"));
            if (answer)
            {
                Settings = new Settings();
                IsEnabled = false;
                IsEditButtonEnabled = true;
            }
        }

        private async Task Save()
        {
            var settings = JsonConvert.DeserializeObject<Settings>(Preferences.Get(Statics.SettingsKey, JsonConvert.SerializeObject(new Settings())));
            var succsave = AppResources.ResourceManager.GetString("succsave");
            var note = AppResources.ResourceManager.GetString("note");
            var newsetting = AppResources.ResourceManager.GetString("newsetting");
            if (settings.Language == Settings.Language)
            {
                PopupMessage.InfoPop(succsave).GetAwaiter();
            }
            else
            PopupMessage.InfoPop(succsave + System.Environment.NewLine + note + System.Environment.NewLine + newsetting).GetAwaiter();
            Preferences.Set(Statics.SettingsKey, JsonConvert.SerializeObject(Settings));
            IsEnabled = false;
            IsEditButtonEnabled = true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

