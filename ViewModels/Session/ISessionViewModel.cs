
using AdminApi;
using System.ComponentModel;

namespace ViewModels;

public interface ISessionViewModel : ILzSessionViewModelAuthNotifications
{
    IAdminApi Admin { get; set; }   
    TenantUsersViewModel TenantUsersViewModel { get; set; }
    SubtenantsViewModel SubtenantsViewModel { get; set; }
    string TenantName { get; set; }
}