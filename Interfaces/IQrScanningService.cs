using System.Threading.Tasks;

namespace InventurApp.Interfaces
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}