using InventurApp.Models;
using InventurApp.Models.ExportModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
namespace InventurApp.ViewModels
{
    class CountedItemListViewModel : INotifyPropertyChanged
    {
        public async Task LoadData()
        {
            
            var items = await App.CountedItemsRepository.GetItems();
            Items = items;
            await Task.CompletedTask;
        }
        private List<ExportArticle> _items { get; set; }
        public List<ExportArticle> Items
        {
            get => _items;
            set
            {
                _items = value;
                NotifyPropertyChanged("Items");
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
