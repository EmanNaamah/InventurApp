using InventurApp.Models.ImportModels;
using System.Threading.Tasks;

namespace InventurApp.Repositories.GenericRepository
{
    interface IImportRepository :IRepositoryBase<Article>
    {
        Task<Article> GetItem(string articleNr);
    }
}
