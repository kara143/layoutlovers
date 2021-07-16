using System.Collections.Generic;
using MvvmHelpers;
using layoutlovers.Models.NavigationMenu;

namespace layoutlovers.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}