using System;
using System.Collections.Generic;
using System.Text;

namespace InventurApp.Models.ExportModels
{
    public class UploadRespons
    {
        public int CountReaded { get; set; }
        public int CountError { get; set; }
        public List<string> ErrorList { get; set; }
        public int HttpStatusCode { get; set; }
        public string Message { get; set; }
      
    }
}
