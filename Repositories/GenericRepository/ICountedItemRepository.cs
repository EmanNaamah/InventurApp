using InventurApp.Models.ExportModels;
using System.Threading.Tasks;

namespace InventurApp.Repositories.GenericRepository
{
    interface ICountedItemRepository:IRepositoryBase<ExportArticle>
    {
        Task<double> GetItemLot(string articleBarcode);
        Task<bool> SearchItem(string articleBarcode);
    }
}
