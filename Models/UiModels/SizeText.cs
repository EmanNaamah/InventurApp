namespace InventurApp.Models
{
    public class SizeText
    {
        public string Measuringunit1TextCaption { get; set; }
        public string Measuringunit2TextCaption { get; set; } = "";
        public string SizeTextCaption1 { get; set; } = "";
        public string SizeTextCaption2 { get; set; } = "";
        public string SizeTextCaption3 { get; set; } = "";
        public string SizeTextCaption4 { get; set; } = "";
        public bool SizeText1Enabled => !string.IsNullOrEmpty(SizeTextCaption1); 
        public bool SizeText2Enabled => !string.IsNullOrEmpty(SizeTextCaption2);
        public bool SizeText3Enabled => !string.IsNullOrEmpty(SizeTextCaption3);
        public bool SizeText4Enabled => !string.IsNullOrEmpty(SizeTextCaption4);
        public bool Measuringunit1Enabled => !string.IsNullOrEmpty(Measuringunit1TextCaption);
        public bool Measuringunit2Enabled => !string.IsNullOrEmpty(Measuringunit2TextCaption);
    }
}
