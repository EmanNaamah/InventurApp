namespace InventurApp.Models
{
    public class ArticleModel
        {
        public string ArticleNumber { get; set; } = "";
        public string Charge { get; set; }
        public double Measuringunit1 { get; set; }
        public double Measuringunit2 { get; set; }
        public string Descreption { get; set; } = "";
        public string Serialnumber { get; set; } = "";
        public string AlreadyCounted { get; set; } = "";
        public double SizeText1value { get; set; } 
        public double SizeText2value { get; set; } 
        public double SizeText3value { get; set; } 
        public double SizeText4value { get; set; }
        public bool ChargeTextEnabled { get; set; } = false;
        public bool AddSerButtonEnabled { get; set; }
        public string KZLand { get; set; } = "";
        public bool IsKZLandEnable { get; set; }

    }
}
