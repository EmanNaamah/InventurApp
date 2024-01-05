using InventurApp.Models.ImportModels;
using InventurApp.Models.UiModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventurApp.Repositories.GenericRepository
{
    public class ImportRepository:IImportRepository
    {
        public ImportRepository(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Article>().Wait();
        }
        
        readonly SQLiteAsyncConnection _database;
        public async Task<bool> DeleteAll()
        {
            try
            {
                var items = await _database.Table<Article>().ToListAsync();
                foreach (var item in items)
                {
                    await _database.Table<Article>().DeleteAsync(x => x.Artikelnummer == item.Artikelnummer);
                }
                return await Task.FromResult(true);

            }
            catch (Exception ex)
            {
                PopupMessage.ErrorPop(ex.Message.ToString()).GetAwaiter();
                return false;
            }
             }
        public async Task<List<Article>> GetItems()
        {
            try
            {
                return await _database.Table<Article>().ToListAsync();
            }
            catch (Exception ex)
            {
                PopupMessage.ErrorPop(ex.Message.ToString()).GetAwaiter();
                return null;
            }
           
        }
        public async Task<int> SaveItem(Article article)
        {
            try
            {
                await _database.InsertAsync(article);
                var dbItems = await _database.Table<Article>().ToListAsync();
                return dbItems.Count;
            }
            catch (Exception ex)
            {
                PopupMessage.ErrorPop(ex.Message.ToString()).GetAwaiter();
                return 0;
            }
           
        }
        public async Task<int> SaveItems(List<Article> articles)
        {
            try
            {
                foreach (var item in articles)
                {
                    await _database.InsertAsync(item);
                }
                var dbItems = await _database.Table<Article>().ToListAsync();
                return dbItems.Count;
            }
            catch (Exception ex)
            {
                PopupMessage.ErrorPop(ex.Message.ToString()).GetAwaiter();
                return 0;
            }
        }
        public async Task<Article> GetItem(string articleNr)
        {
            try
            {
                return await _database.Table<Article>().FirstOrDefaultAsync(x => x.Artikelnummer == articleNr);
            }
            catch (Exception ex)
            {
                PopupMessage.ErrorPop(ex.Message.ToString()).GetAwaiter();
                return null;
            }
           
        }
    }
}
