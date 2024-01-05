using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventurApp.Repositories.GenericRepository
{
    public interface IRepositoryBase<T>
    {
        Task<int> SaveItem(T article);
        Task<bool> DeleteAll();
        Task<int> SaveItems(List<T> articles);
        Task<List<T>> GetItems();
    }
}
