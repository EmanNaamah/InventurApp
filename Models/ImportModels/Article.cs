
using System;
using System.Collections.Generic;

namespace InventurApp.Models.ImportModels
{

    public class ArticleData
    {
        public List<Article> data { get; set; }
    }

    public class Article
    {
        public string KZWarengruppe { get; set; }
        public string KZArtikelgruppe { get; set; }
        public string KZUnterArtikelgruppe { get; set; }
        public int? ArtikelUntergruppeID { get; set; } 
        public string Artikelnummer { get; set; } 
        public string ArtBezeichnung1 { get; set; } 
        public string KZArtMengeneinheit1 { get; set; } 
        public string KZArtMengeneinheit2 { get; set; } 
        public string WgrTextAbmasse1 { get; set; } 
        public string WgrTextAbmasse2 { get; set; } 
        public string WgrTextAbmasse3 { get; set; } 
        public string WgrTextAbmasse4 { get; set; }
        public bool ArtSeriennummernfaehigJN { get; set; } 
        public bool ArtChargenfaehigJN { get; set; }
        public bool ArtUrsprungsnachweisJN  { get; set; }
        public string ArtEAN { get; set; }
        public string ArtEdiKennung { get; set; }
        public string ArtBarcode { get; set; }
        
    }
}