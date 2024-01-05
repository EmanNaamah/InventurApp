using InventurApp.ViewModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InventurApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ArtikellistPage : ContentPage
    {
        public ArtikellistPage()
        {
            InitializeComponent();
        }
        protected async override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            await (this.BindingContext as ArtikellistViewModel).LoadData();
            await Task.Delay(1000);
            foreach (var column in grid1.Columns)
                column.Width = 150;
        }
    }
}