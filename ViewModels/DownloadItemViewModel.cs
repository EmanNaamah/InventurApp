using InventurApp.Culture;
using InventurApp.Models.UiModels;
using InventurApp.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace InventurApp.ViewModels
{
    public class DownloadItemViewModel:INotifyPropertyChanged
    {
        public ArticleService ArticleService { get; set; }
        public DownloadItemViewModel()
        {
             ArticleService = new ArticleService();
            LoadAllFilterItemsCommand = new Command(async () => await LoadAllFilterItemss());
            LoadGoodsCommand = new Command(async () => await LoadGoodsGroups());
            ResetCommand = new Command(() => Reset());
            LoadArticleGroupsCommand = new Command(async () => await LoadArticleGroupsGroups());
            LoadUnderArticleGroupsCommand = new Command(async () => await LoadUnderArticleGroups());
            LoadGoodsCommand.Execute(null);
        }
   
        private void Reset()
        {
            SelectedArticleGroup = null;
            SelectedGoodsGroup = null;
            SelectedUnderArticleGroup = null;
            ArticleGroups = new List<string>();
            UnderArticleGroups = new List<string>();
            GoodsGroups = new List<string>();
            LoadGoodsCommand.Execute(null);
            IsArticleGroupsEnabled = false;
            IsUnderArticleGroupsEnabled = false;
        }
        private async Task LoadAllFilterItemss()
        {
            try
            {
                IsBusy = true;
                var allfilterItems = await ArticleService.GetAllFilteItems(SelectedGoodsGroup, SelectedArticleGroup, SelectedUnderArticleGroup);
                var ListCount = allfilterItems.data.Count();
                if (allfilterItems != null)
                {
                    
                    await App.ImportRepository.DeleteAll();
                    var importedItems = await App.ImportRepository.SaveItems(allfilterItems.data); 
                    IsBusy = false;
                    PopupMessage.InfoPop($"Imported ' {importedItems} ' Items !").GetAwaiter();
                    await Shell.Current.GoToAsync("MenuPage");
                    Reset();
                }
                else
                {
                    IsBusy = false;
                    PopupMessage.ErrorPop(AppResources.ResourceManager.GetString("item_not_found_dialog_title")).GetAwaiter();
                }
            }
            catch (System.Exception ex)
            {

                IsBusy = false;
                PopupMessage.ErrorPop(ex.Message.ToString()).GetAwaiter();
            }

        }
        private async Task LoadUnderArticleGroups()
        {
            var underArticleGroups = await ArticleService.GetUnterArticleGroups(SelectedGoodsGroup,SelectedArticleGroup);
            if (underArticleGroups.Count != 0 && underArticleGroups != null) IsUnderArticleGroupsEnabled = true;
            else IsUnderArticleGroupsEnabled = false;
            UnderArticleGroups = underArticleGroups;
        }
        private async Task LoadArticleGroupsGroups()
        {
            var articleGroups = await ArticleService.GetArticleGroups(SelectedGoodsGroup);
            if (articleGroups != null && articleGroups.Count != 0) IsArticleGroupsEnabled = true;
            else IsArticleGroupsEnabled = false;
            ArticleGroups = articleGroups;
        }
        private async Task LoadGoodsGroups()
        {
            try
            {
                IsBusy = true;
                var goodsResult = await ArticleService.GetGoodsGroups();
                if (goodsResult != null) { IsBusy = false; IsGoodsGroupsEnabled = true; }
                else {
                    IsBusy = false;  IsGoodsGroupsEnabled = false;
                    PopupMessage.ErrorPop(AppResources.ResourceManager.GetString("checkInternetConnection")).GetAwaiter();
                }
                GoodsGroups = goodsResult;
            }
            catch (System.Exception ex)
            {
                IsBusy = false; 
                PopupMessage.ErrorPop(ex.Message.ToString()).GetAwaiter();
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
        public ICommand ResetCommand { get; set; }
        public ICommand LoadAllFilterItemsCommand { get; set; }
        public ICommand LoadGoodsCommand { get; set; }

        public ICommand LoadArticleGroupsCommand { get; set; }

        public ICommand LoadUnderArticleGroupsCommand { get; set; }

        private List<string> _goodsGroups { get; set; }
        public List<string> GoodsGroups 
        {
            get => _goodsGroups;
            set
            {
                _goodsGroups = value;
                NotifyPropertyChanged("GoodsGroups");
            }
        }
        private List<string> _articleGroups { get; set; }
        public List<string> ArticleGroups 
        {
            get => _articleGroups;
            set
            {
                _articleGroups = value;
                NotifyPropertyChanged("ArticleGroups");
            }
        }
        private List<string> _underArticleGroups { get; set; }
        public List<string> UnderArticleGroups {
            get => _underArticleGroups;
            set
            {
                _underArticleGroups = value;
                NotifyPropertyChanged("UnderArticleGroups");
            }
        }

        private string _selectedGoodsGroup { get; set; }
        public string SelectedGoodsGroup {
            get => _selectedGoodsGroup;
            set
            {
                _selectedGoodsGroup = value;
                LoadArticleGroupsCommand.Execute(null);
                SelectedArticleGroup = null;
                SelectedUnderArticleGroup = null;
                NotifyPropertyChanged("SelectedGoodsGroup");
            }
        }
        private string _selectedArticleGroup { get; set; }
        public string SelectedArticleGroup {
            get => _selectedArticleGroup;
            set
            {
                _selectedArticleGroup = value;
                SelectedUnderArticleGroup = null;
                LoadUnderArticleGroupsCommand.Execute(null);
                NotifyPropertyChanged("SelectedArticleGroup");
            }
        }
        private string _selectedUnderArticleGroup { get; set; }
        public string SelectedUnderArticleGroup {
            get => _selectedUnderArticleGroup;
            set
            {
                _selectedUnderArticleGroup = value;
                NotifyPropertyChanged("SelectedUnderArticleGroup");
            }
        }
        public bool _isGoodsGroupsEnabled { get; set; }
        public bool IsGoodsGroupsEnabled {
            get => _isGoodsGroupsEnabled;
            set
            {
                _isGoodsGroupsEnabled = value;
                NotifyPropertyChanged("IsGoodsGroupsEnabled");
            }
        }
        public bool _isArticleGroupsEnabled { get; set; }
        public bool IsArticleGroupsEnabled {
            get => _isArticleGroupsEnabled;
            set
            {
                _isArticleGroupsEnabled = value;
                NotifyPropertyChanged("IsArticleGroupsEnabled");
            }
        }
        private bool _isUnderArticleGroupsEnabled { get; set; }
        public bool IsUnderArticleGroupsEnabled {
            get => _isUnderArticleGroupsEnabled;
            set
            {
                _isUnderArticleGroupsEnabled = value;
                NotifyPropertyChanged("IsUnderArticleGroupsEnabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName = "")
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
