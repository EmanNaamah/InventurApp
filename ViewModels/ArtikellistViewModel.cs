using InventurApp.Models.ImportModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace InventurApp.ViewModels
{
    public class ArtikellistViewModel : INotifyPropertyChanged
    {
        
        public async Task LoadData()
        {
            var items = await App.ImportRepository.GetItems();
            Articles = items;
            await Task.CompletedTask;
        }

        private List<Article> _articles { get; set; }
        public List<Article> Articles { 
                get => _articles;
                set
                {
                _articles = value;
                 NotifyPropertyChanged("Articles");
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
