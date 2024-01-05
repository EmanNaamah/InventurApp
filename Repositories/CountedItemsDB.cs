using InventurApp.Models.ExportModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventurApp.Repositories
{
   public class CountedItemsDB
    {
        readonly SQLiteAsyncConnection _database;
        public CountedItemsDB(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ExportArticle>().Wait();
        }
        public  async Task<List<ExportArticle>> GetItemsAsync()
        {
           return await _database.Table<ExportArticle>().ToListAsync();
          
        }
        public async Task<double> GetItemLot(string articleBarcode)
        {
            var items = await  _database.Table<ExportArticle>().Where(x => x.ArticleNumber == articleBarcode).ToListAsync();
            var count = 0.0;
            foreach (var item in items)
            {
             
                count += Convert.ToInt32(item.Qtyunit2);
            }
            return count;
        }
        public async Task<bool> SearchItem(string articleBarcode)
        {
            var alreadyCounted = await _database.Table<ExportArticle>().FirstOrDefaultAsync(x => x.ArticleNumber == articleBarcode);
            if (alreadyCounted != null)
                return await Task.FromResult(true);
            else return await Task.FromResult(false); 
        }
        public async Task<bool> DeleteAllItemAsync()
        {
            var items = await _database.Table<ExportArticle>().ToListAsync();
            foreach (var item in items)
            {
                await _database.Table<ExportArticle>().DeleteAsync(x => x.ArticleNumber == item.ArticleNumber);
            }
            return await Task.FromResult(true);
        }
        public async Task<int> SaveItemAsync(ExportArticle item)
        {
            await _database.InsertAsync(item);
            var dbItems = await _database.Table<ExportArticle>().ToListAsync();
            return dbItems.Count;
        }
        public async Task<int> SaveItemsAsync(List<ExportArticle> items)
        {
            foreach (var item in items)
            {
                await _database.InsertAsync(item);
            }
            var dbItems = await _database.Table<ExportArticle>().ToListAsync();
            return dbItems.Count;
        }
    }
}

