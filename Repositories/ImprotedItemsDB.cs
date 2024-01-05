using System.Collections.Generic;
using System.Threading.Tasks;
using InventurApp.Models;
using InventurApp.Models.ImportModels;
using SQLite;
namespace InventurApp.Repositories
{
    public class ImprotedItemsDB
    {
        readonly SQLiteAsyncConnection _database;
        public ImprotedItemsDB(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Article>().Wait();
        }
        public async  Task<List<Article>> GetItemsAsync()
        {
            return await _database.Table<Article>().ToListAsync();
        }
        public async Task<Article> GetItemAsync(string articleBarcode)
        {
            return await _database.Table<Article>().FirstOrDefaultAsync(x=>x.Artikelnummer==articleBarcode);
        }
        public async Task<bool> DeleteAllItemAsync()
        {
            var items = await _database.Table<Article>().ToListAsync();
            foreach (var item in items)
            {
                await _database.Table<Article>().DeleteAsync(x =>x.Artikelnummer==item.Artikelnummer);
            }
            return await Task.FromResult(true);
        }
        public async Task<int> SaveItemsAsync(List<Article> items)
        {
            foreach (var item in items)
            {
                await _database.InsertAsync(item);
            }
            var dbItems = await _database.Table<Article>().ToListAsync();
            return dbItems.Count;
        }
        public async Task<int> SaveItemAsync(Article item)
        {
            await _database.InsertAsync(item);
            var dbItems = await _database.Table<Article>().ToListAsync();
            return dbItems.Count;
        }
    }
}
