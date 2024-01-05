using System;
using System.Collections.Generic;

namespace InventurApp.Models.ExportModels
{
    public class InventoryData
    {
        public List<Inventory> Data { get; set; }
    }
    public class Inventory
    {
        public int InventurKopfID { get; set; }
        public int InkArt { get; set; }
        public DateTime InkStichtag { get; set; }
        public string InkBemerkung { get; set; }

    }
}
