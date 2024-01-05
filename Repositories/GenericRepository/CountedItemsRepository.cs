using InventurApp.Models.ExportModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventurApp.Repositories.GenericRepository
{
    public class CountedItemsRepository:ICountedItemRepository
    {
        public CountedItemsRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ExportArticle>().Wait();
        }
       
        readonly SQLiteAsyncConnection _database;
        public async Task<int> SaveItem(ExportArticle item)
        {
            await _database.InsertAsync(item);
            var dbItems = await _database.Table<ExportArticle>().ToListAsync();
            return dbItems.Count;
        }
        public async Task<int> SaveItems(List<ExportArticle> exportArticles)
        {
            foreach (var item in exportArticles)
            {
                await _database.InsertAsync(item);
            }
            var dbItems = await _database.Table<ExportArticle>().ToListAsync();
            return dbItems.Count;
        }
        public async Task<bool> DeleteAll()
        {
            var items = await _database.Table<ExportArticle>().ToListAsync();
            foreach (var item in items)
            {
                await _database.Table<ExportArticle>().DeleteAsync(x => x.ArticleNumber == item.ArticleNumber);
            }
            return await Task.FromResult(true);
        }
        public async Task<List<ExportArticle>> GetItems()
        {
            return await _database.Table<ExportArticle>().ToListAsync();
        }
        public async Task<double> GetItemLot(string articleBarcode)
        {
            var items = await _database.Table<ExportArticle>().Where(x => x.ArticleNumber == articleBarcode).ToListAsync();
            double count = 0;
            foreach (var item in items)
                count += Convert.ToDouble(item.Qtyunit1);
            return count;
        }
        public async Task<bool> SearchItem(string articleBarcode)
        {
            var alreadyCounted = await _database.Table<ExportArticle>().FirstOrDefaultAsync(x => x.ArticleNumber == articleBarcode);
            if (alreadyCounted != null)
            return await Task.FromResult(true);
            else return await Task.FromResult(false);
        }
    }
}
