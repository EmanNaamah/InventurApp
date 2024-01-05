using InventurApp.Culture;
using InventurApp.Models.ExportModels;
using InventurApp.Models.ImportModels;
using InventurApp.Models.UiModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InventurApp.Services
{
    public class DatService
    {
        public async Task CreateDatFile(string path)
        {
            try
            {
              
                var articles = await App.CountedItemsRepository.GetItems();
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    var articleList = new List<string>();

                    foreach (var article in articles)
                    {
                        string strLagerGruppe = "", strLagerort = "", strMagazin = "";
                        var storageProperties = article.Storage.Split('#');
                        if(storageProperties.Length < 2)
                            storageProperties = article.Storage.Split('.');
                        if (storageProperties.Length > 1)
                        {
                            if (storageProperties.Length == 3)
                            {
                                strLagerGruppe = storageProperties[0].Trim('#').Trim('.');
                                strLagerort = storageProperties[1].Trim('#').Trim('.') ;
                                strMagazin = storageProperties[2].Trim('#').Trim('.') ;
                            }
                            if (storageProperties.Length == 2)
                            {
                                strLagerGruppe = storageProperties[0].Trim('#').Trim('.'); ;
                                strLagerort = storageProperties[1].Trim('#').Trim('.') ;
                            }
                        }
                      
                        var articleString = "";
                        articleString += (article.UserNumber ?? "") + ";";
                        articleString += (strLagerGruppe ?? "") + ";";
                        articleString += (strLagerort ?? "") + ";";
                        articleString += (strMagazin ?? "") + ";";
                        articleString += (article.ArticleNumber ?? "") + ";";
                        articleString += (article.Qtyunit1.ToString() ?? "") + ";";
                        articleString += (article.Qtyunit2.ToString() ?? "") + ";";
                        articleString += (article.Cf1.ToString() ?? "") + ";";
                        articleString += (article.Cf2.ToString() ?? "") + ";";
                        articleString += (article.Cf3.ToString() ?? "") + ";";
                        articleString += (article.Cf4.ToString() ?? "") + ";";
                        articleString += (article.Serialnumber ?? "") + ";";
                        articleString += (article.Charge ?? "") + ";";
                        articleString += (article.KZLand ?? "") + ";";
                        articleString += article.Date ?? "";
                        articleList.Add(articleString);


                    }

                    var oneSting = string.Join("\n", articleList).ToString();

                    await fs.WriteAsync(Encoding.ASCII.GetBytes(oneSting), 0, oneSting.Length);
                    if (oneSting != null)
                    {
                        PopupMessage.savePop(AppResources.ResourceManager.GetString("export_success_dialog_description")).GetAwaiter(); 
                    }
                   else PopupMessage.savePop(AppResources.ResourceManager.GetString("errorsaveDataBase")).GetAwaiter();
                }
            }

            catch (Exception ex)
            {
                PopupMessage.ErrorPop($"{ex.Message}").GetAwaiter();
            }
         }

        public async Task ImportDatArtikelFile(string path)
        {
            try
            {
                string[] items = File.ReadAllLines(path, System.Text.Encoding.GetEncoding(1252));
                await App.ImportRepository.DeleteAll();
                foreach (var item in items)
                {
                    if (item.Length > 10)
                    {
                        var itemProperties = item.Split(new string[] { ";" }, StringSplitOptions.None);
                        var importeArticle = new Article()
                        {
                            Artikelnummer = itemProperties[0] ?? "",
                            ArtBezeichnung1 = itemProperties[1] ?? "",
                            KZArtMengeneinheit1 = itemProperties[2] ?? "",
                            KZArtMengeneinheit2 = itemProperties[3] ?? "",
                            WgrTextAbmasse1 = itemProperties[4] ?? "",
                            WgrTextAbmasse2 = itemProperties[5] ?? "",
                            WgrTextAbmasse3 = itemProperties[6] ?? "",
                            WgrTextAbmasse4 = itemProperties[7] ?? "",
                            ArtSeriennummernfaehigJN = Convert.ToBoolean(Convert.ToInt32(itemProperties[8])),
                            ArtChargenfaehigJN = Convert.ToBoolean(Convert.ToInt32(itemProperties[9])),
                            //wait to creat neu File with the neu prop ...
                            ArtUrsprungsnachweisJN = Convert.ToBoolean(Convert.ToInt32(itemProperties[10])),  
                            ArtEAN = itemProperties[11] ?? "",
                            ArtBarcode = itemProperties[12] ?? "",
                            
                        };

                        await App.ImportRepository.SaveItem(importeArticle);
                    }

                }
                await App.Current.MainPage.DisplayAlert(AppResources.ResourceManager.GetString("import_success_dialog_title1"), $"{items.Length} {AppResources.ResourceManager.GetString("import_success_dialog_description1")}  ", "OK");

            }
            catch (Exception ex)
            {

             PopupMessage.ErrorPop($"{ex.Message}").GetAwaiter();

            }

        }
    }
}
