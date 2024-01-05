using System;

namespace InventurApp.Models.ExportModels
{
    //export Article
   public class Stocktaking
    {
        public string UserNumber { get; set; }
        public string Storage1 { get; set; }
        public string Storage2 { get; set; }
        public string Storage3 { get; set; }
        public string ArticleNumber { get; set; }
        public double Qty1 { get; set; }
        public double Qty2 { get; set; }
        public string Date { get; set; }
        public double Cf1 { get; set; }
        public double Cf2 { get; set; }
        public double Cf3 { get; set; }
        public double Cf4 { get; set; }
        public string Lot { get; set; }
        public dynamic serialNumber { get; set; }
        public string countryoforigin { get; set; }
    }
}
