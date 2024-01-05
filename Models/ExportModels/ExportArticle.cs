using SQLite;
using System;

namespace InventurApp.Models.ExportModels
{
    // invertorList Artilcle
    public class ExportArticle
    {
        public string UserNumber { get; set; }
        public string ArticleNumber { get; set; }
        public string Storage { get; set; }
        public double Qtyunit1 { get; set; }
        public double Qtyunit2 { get; set; }
        public string Date { get; set; }
        public double Cf1 { get; set; }
        public double Cf2 { get; set; }
        public double Cf3 { get; set; }
        public double Cf4 { get; set; }
        public string Serialnumber { get; set; }
        public string Charge { get; set; }
        public string KZLand { get; set; }


    }
}
