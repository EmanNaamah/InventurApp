using System.Threading.Tasks;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace InventurApp.Services
{
    public static class PermissionUtils
    {
        public static async Task<bool> GetPermission<TPermission>() where TPermission : BasePermission, new()
        {
            var hasPermission = await Permissions.CheckStatusAsync<TPermission>();

            if (hasPermission == PermissionStatus.Granted)
                return true;
            else if (hasPermission == PermissionStatus.Disabled)
                return false;

            var result = await Permissions.RequestAsync<TPermission>();
            if (result != PermissionStatus.Granted)
                return false;

            return true;
        }
    }
}
