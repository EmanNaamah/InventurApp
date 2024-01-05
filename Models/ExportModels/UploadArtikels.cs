using System;
using System.Collections.Generic;
using System.Text;

namespace InventurApp.Models.ExportModels
{//upload article
    public class UploadArtikels
    {
        public string StocktakingID { get; set; }
        public List<Stocktaking> Stocktakings { get; set; }
       
    }
}
