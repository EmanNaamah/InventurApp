using InventurApp.Repositories;
using InventurApp.Repositories.GenericRepository;
using InventurApp.Services;
using System;
using System.IO;
using Xamarin.Forms;


namespace InventurApp
{
    public partial class App : Application
    {
        static ImportRepository improtedItemsDB;
        public static ImportRepository ImportRepository
        {
            get
            {
                if (improtedItemsDB == null)
                {
                    improtedItemsDB = new ImportRepository(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "items.db3"));
                   // improtedItemsDB = new ImprotedItemsDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "items.db3"));
                }
                return improtedItemsDB;
            }
        }
        static CountedItemsRepository countedItemsDB;
        public static CountedItemsRepository CountedItemsRepository
        {
            get
            {
                if (countedItemsDB == null)
                {
                    countedItemsDB = new CountedItemsRepository(Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Export.db3"));
                }
                return countedItemsDB;
            }
        }
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

      

     
    }
}
