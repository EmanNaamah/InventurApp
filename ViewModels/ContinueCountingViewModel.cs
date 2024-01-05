using InventurApp.Culture;
using InventurApp.Models;
using InventurApp.Models.ExportModels;
using InventurApp.Models.ImportModels;
using InventurApp.Models.UiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InventurApp.ViewModels
{
    class ContinueCountingViewModel : INotifyPropertyChanged
    {
        public ContinueCountingViewModel()
        {
            ArticleModel = new ArticleModel();
            Sizes = new SizeText();
            GetAllSettings();
            SearchArticleCommand = new Command(async x => await GetArticle());
            SaveCommand = new Command(async x => await AddNewExportArtikel());
            ResetCommand = new Command(() => Reset());
           // IsLagerCodeAvailabel = new Command(async x => await IsLagerAvailabel());
            AddserialNr = new Command(() => TestSiriNR(NewSerialNumber));
            SerialNumberList = MyStorage.SerialNumbersList;
            MaximalSerialNR = MyStorage.MaxSiNrCount.ToString();
            isSerNrCountcorrect = false;
            TestSiriNrCountbeforBack = new Command(async x => await testSiriNrCountbeforBack());
        }
        public void TestSiriNR(string newSerialNumber)
        {
            if (MyStorage.MaxSiNrCount == 0 || SerialNumberList.Count < MyStorage.MaxSiNrCount || isSerNrCountcorrect == true)
            {
                if (!String.IsNullOrEmpty(newSerialNumber))
                    SaveSiriNr(newSerialNumber);
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                    {
                        var answer = await App.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("Alert"), AppResources.ResourceManager.GetString("askToSeriNRAmount"), AppResources.ResourceManager.GetString("yes"), AppResources.ResourceManager.GetString("no"));
                        if (answer)
                        {
                            if (!String.IsNullOrEmpty(newSerialNumber))
                            {
                                isSerNrCountcorrect = true;
                                SaveSiriNr(newSerialNumber);
                                MyStorage.MaxSiNrCount = 0;
                            }

                        }
                        else NewSerialNumber = "";
                    });
            }


        }
        public async Task testSiriNrCountbeforBack()
        {
            if (SerialNumberList.Count != MyStorage.MaxSiNrCount && MyStorage.MaxSiNrCount != 0)
            {
                var answer = await App.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("Alert"), AppResources.ResourceManager.GetString("askToSeriNRAmount"), AppResources.ResourceManager.GetString("yes"), AppResources.ResourceManager.GetString("no"));
                if (!answer)
                {
                    await Shell.Current.GoToAsync("AddSerialNummerPage");
                }
              
            }
           
        }
        public void SaveSiriNr(string seriNr)
        {
            SerialNumber serialNumber = new SerialNumber()
            {
                SerialNummber = seriNr
            };
            SerialNumberList.Add(serialNumber);
            var clone = SerialNumberList;
            SerialNumberList = null;
            SerialNumberList = clone;
            MyStorage.SerialNumbersList = SerialNumberList;
            NewSerialNumber = "";
        }
        public void DeleteserialNummer(string serialNumber)
        {
            var serialToDelete = SerialNumberList.First(x => x.SerialNummber == serialNumber);
            SerialNumberList.Remove(serialToDelete);
            var clone = SerialNumberList;
            SerialNumberList = null;
            SerialNumberList = clone;
            MyStorage.SerialNumbersList = SerialNumberList;
            NewSerialNumber = "";
        }
        public ICommand AddserialNr { get; set; }
        public ICommand TestSiriNrCountbeforBack { get; set; }
        public string maximalSerialNR { get; set; }
        public ICommand SearchArticleCommand { get; set; }
        public Command SaveCommand { get; }
        public ICommand ResetCommand { get; }
        //public ICommand IsLagerCodeAvailabel { get; }
        public ICommand RefreshCommand { get; set; }
        private string _searchedArticleNumber { get; set; }
        public string SearchedArticleNumber
        {
            get => _searchedArticleNumber;
            set
            {
                _searchedArticleNumber = value;
                NotifyPropertyChanged("SearchedArticleNumber");
            }
        }
        private string _newSerialNumber { get; set; }
        public string NewSerialNumber
        {
            get => _newSerialNumber;
            set
            {
                _newSerialNumber = value;
                NotifyPropertyChanged("NewSerialNumber");
            }
        }
        private List<SerialNumber> _serialNumberList { get; set; }
        public List<SerialNumber> SerialNumberList
        {
            get => _serialNumberList;
            set
            {
                _serialNumberList = value;
                NotifyPropertyChanged("SerialNumberList");
            }
        }
        private string _maximalSerialNR { get; set; }
        public string MaximalSerialNR
        {
            get => _maximalSerialNR;
            set
            {
                _maximalSerialNR = value;
                NotifyPropertyChanged("MaximalSerialNR");
            }
        }

        public bool isSerNrCountcorrect;
        private SizeText _sizes { get; set; }
        public SizeText Sizes
        {
            get => _sizes;
            set
            {
                _sizes = value;
                NotifyPropertyChanged("Sizes");
            }
        }
        private bool _isBusy { get; set; }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                NotifyPropertyChanged("IsBusy");
            }
        }
        public LoginSettings LoginSettings { get; set; }
        public Settings Settings { get; set; }
        public string StorageId { get; set; } = "";
        public string UserNumber { get; set; } = "";
        public Article Article { get; set; }
        //public async Task<bool> IsLagerAvailabel()
        //{
        //    if (String.IsNullOrEmpty(LoginSettings.StorageId) || String.IsNullOrEmpty(LoginSettings.UserNumber))
        //    {
        //        await Application.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("Error"), AppResources.ResourceManager.GetString("not_valid_StoregId"), "OK"); 
        //        await Shell.Current.GoToAsync("NewCountingPage");
        //        return await Task.FromResult(false);
        //    }
               
        //    else return await Task.FromResult(true);
        //}
        private async Task AddNewExportArtikel()
        {
            try
            {
                MyStorage.MaxSiNrCount = 0;
                if (ArticleModel.AddSerButtonEnabled && ArticleModel.Measuringunit1 > 0)
                    MyStorage.MaxSiNrCount = ArticleModel.Measuringunit1;
                if (ArticleModel.AddSerButtonEnabled && MyStorage.SerialNumbersList.Count == 0)
                    await Shell.Current.GoToAsync("AddSerialNummerPage");
                else if (ArticleModel.AddSerButtonEnabled && ArticleModel.Measuringunit1 != MyStorage.SerialNumbersList.Count)
                {
                   var answer = await App.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("askToSeriNRAmount"), null, AppResources.ResourceManager.GetString("yes"), AppResources.ResourceManager.GetString("no"));
                  if (answer)
                  {
                     ArticleModel.Measuringunit1 = MyStorage.SerialNumbersList.Count;
                  }
                    else
                        await Shell.Current.GoToAsync("AddSerialNummerPage");
                }
               else if (ArticleModel.AddSerButtonEnabled && MyStorage.SerialNumbersList.Count > 0 && ArticleModel.Measuringunit1 == MyStorage.SerialNumbersList.Count)
                {
                    foreach (var serilnummer in MyStorage.SerialNumbersList)
                    {
                        ExportArticle item = new ExportArticle()
                        {
                            ArticleNumber = SearchedArticleNumber ?? "",
                            Storage = StorageId ?? "",
                            Charge = ArticleModel.Charge ?? "",
                            Qtyunit1 = 1,
                            Qtyunit2 = ArticleModel.Measuringunit2 ,
                            Cf1 = ArticleModel.SizeText1value ,
                            Cf2 = ArticleModel.SizeText2value ,
                            Cf3 = ArticleModel.SizeText3value ,
                            Cf4 = ArticleModel.SizeText4value ,
                            Serialnumber = serilnummer.SerialNummber ?? "",
                            KZLand = ArticleModel.KZLand ?? "",
                            UserNumber = UserNumber ?? "",
                            Date = DateTime.Now.ToString("yyyyMMddhhmmss"),
                        };
                        if (item != null)
                        {
                            await App.CountedItemsRepository.SaveItem(item);
                        }
                    }
                    Reset();
                }
                else if (!ArticleModel.AddSerButtonEnabled)
                {
                    ExportArticle item = new ExportArticle()
                    {
                        ArticleNumber = SearchedArticleNumber ?? "",
                        Storage = StorageId ?? "",
                        Charge = ArticleModel.Charge ?? "",
                        Qtyunit1 = ArticleModel.Measuringunit1 ,
                        Qtyunit2 = ArticleModel.Measuringunit2 ,
                        Cf1 = ArticleModel.SizeText1value,
                        Cf2 = ArticleModel.SizeText2value,
                        Cf3 = ArticleModel.SizeText3value,
                        Cf4 = ArticleModel.SizeText4value,
                        Serialnumber = "",
                        KZLand = ArticleModel.KZLand ?? "",
                        UserNumber = UserNumber ?? "",
                        Date = DateTime.Now.ToString("yyyyMMddhhmmss"),
                    };
                    if (item != null)
                    {
                        await App.CountedItemsRepository.SaveItem(item);
                    }
                    Reset();
                }

            }
            catch (Exception ex)
            {
                PopupMessage.ErrorPop(ex.Message.ToString()).GetAwaiter();
            }
           
        }
        private void Reset()
        {
            SearchedArticleNumber = "";
            ArticleModel = new ArticleModel();
            Sizes = new SizeText();
            MyStorage.MaxSiNrCount = 0;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public async Task GetArticle()
        {
            try
            {
                IsBusy = true;
            
                SerialNumberList = new List<SerialNumber>();
                var Article = await App.ImportRepository.GetItem(SearchedArticleNumber.ToUpper().Trim(' '));
                var isalreadyCounted = await App.CountedItemsRepository.GetItemLot(SearchedArticleNumber.ToUpper().Trim(' '));
                if (Article != null)
                {
                    IsBusy = false;
                    ArticleModel = new ArticleModel()
                    {
                        ChargeTextEnabled = Article.ArtChargenfaehigJN,
                        Descreption = Article.ArtBezeichnung1,
                        ArticleNumber = Article.KZWarengruppe,
                        AlreadyCounted = isalreadyCounted.ToString(),
                        AddSerButtonEnabled = Article.ArtSeriennummernfaehigJN,
                        IsKZLandEnable = Article.ArtUrsprungsnachweisJN,
                        
                    };
                    Sizes = new SizeText()
                    {
                        Measuringunit1TextCaption = Article.KZArtMengeneinheit1 ?? AppResources.ResourceManager.GetString("menge"),
                        Measuringunit2TextCaption= Article.KZArtMengeneinheit2,
                        SizeTextCaption1 = Article.WgrTextAbmasse1,
                        SizeTextCaption2 = Article.WgrTextAbmasse2,
                        SizeTextCaption3 = Article.WgrTextAbmasse3,
                        SizeTextCaption4 = Article.WgrTextAbmasse4,
                    };
                 
                }
                else
                {
                    IsBusy = false;
                    PopupMessage.ErrorPop(AppResources.ResourceManager.GetString("item_not_found_dialog_title")).GetAwaiter();
                    Reset();
                }
                MyStorage.SerialNumbersList = new List<SerialNumber>();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                PopupMessage.ErrorPop(ex.Message.ToString()).GetAwaiter();

            }
        }
        private void GetAllSettings()
        {
            Settings = JsonConvert.DeserializeObject<Settings>(Preferences.Get(Statics.SettingsKey, JsonConvert.SerializeObject(new Settings())));
            LoginSettings = JsonConvert.DeserializeObject<LoginSettings>(Preferences.Get(Statics.LoginSettings, JsonConvert.SerializeObject(new LoginSettings())));
            UserNumber = LoginSettings.UserNumber;
            StorageId = LoginSettings.StorageId;

        }
        private ArticleModel _articleModel { get; set; }
        public ArticleModel ArticleModel
        {
            get => _articleModel;
            set
            {
                _articleModel = value;
                NotifyPropertyChanged("ArticleModel");
            }
        }
        private void NotifyPropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class SerialNumber {
       public string SerialNummber { get; set; }
    }
    public class MyStorage
    {
        public static double? MaxSiNrCount { get; set; }
        public static List<SerialNumber> SerialNumbersList { get; set; } = new List<SerialNumber>();

    }

}


