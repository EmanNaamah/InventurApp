using InventurApp.Models;
using InventurApp.Models.ImportModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventurApp.Services
{
    public class ArticleService
    {
        public ArticleService()
        {
            ClientService = new ClientService();
        }
        public ClientService ClientService { get; set; }

        #region import
        public async Task<List<string>> GetGoodsGroups()
        {
            var allArticles = await ClientService.DownloadArticles();
            if (allArticles != null) { return allArticles.data.Select(x => x.KZWarengruppe).Distinct().ToList(); }
            else return null;
        }
        public async Task<List<string>> GetArticleGroups(string goodsGroup)
        {
            var filter = $"KZWarengruppe='{goodsGroup}'";
            var allArticles = await ClientService.DownloadArticles(filter);
            var filteredArticles= allArticles.data.Where(x => !string.IsNullOrEmpty(x.KZArtikelgruppe?.Trim()));
            return filteredArticles.Select(x => x.KZArtikelgruppe).Distinct().ToList();
        }
        public async Task<List<string>> GetUnterArticleGroups(string goodsGroup, string articleGroup)
        {
            var filter = $"KZWarengruppe='{goodsGroup}' AND KZArtikelgruppe='{articleGroup}'";
            var allArticles = await ClientService.DownloadArticles( filter);
            return allArticles.data.Where(x=>!string.IsNullOrEmpty(x.KZUnterArtikelgruppe?.Trim())).Select(x => x.KZUnterArtikelgruppe).Distinct().ToList();
        }
        public async Task<ArticleData> GetAllFilteItems(string goodsGroup, string articleGroup,string underArticleGroup)
        {
            var filter = "";
            if (!string.IsNullOrEmpty(goodsGroup)) 
            {
                filter = $"KZWarengruppe='{goodsGroup}'";
                if (!string.IsNullOrEmpty(articleGroup))
                {
                    filter += $"AND KZArtikelgruppe='{articleGroup}'";
                    if (!string.IsNullOrEmpty(underArticleGroup))
                    {
                        filter += $"AND KZUnterArtikelgruppe='{underArticleGroup}'";
                    }
                }
             }
            var allArticles = await ClientService.DownloadArticles( filter);
            return allArticles;
        }

        #endregion import

        #region export


        //Export Methods
        #endregion export
    }
}
