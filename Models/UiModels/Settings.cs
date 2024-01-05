
namespace InventurApp.Models
{
    public class Settings
    {
        public string ServerUrl { get; set; } = "https://erp4all-webapi.erp4all.com";
        public int Port { get; set; } = 3309;
        public string DeviceId { get; set; } = "000";
        public string APIKey { get; set; } = "MiCLASAPIKey";
        public string APIKeyPassword { get; set; } = "TestTest";
        public int LogoutSeconds { get; set; } = 20;
        public string UserPassword { get; set; } = "123";
        public bool IsPrivate { get; set; }

        public bool PZE { get; set; } = true;

        public bool BDE { get; set; } = true;

        public bool FinishedRegistiring { get; set; } = true;

        public Language Language { get; set; }


    }
}
