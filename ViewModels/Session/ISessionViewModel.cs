
using AdminApi;
using System.ComponentModel;

namespace ViewModels;

public interface ISessionViewModel : IBaseAppSessionViewModel
{
    TenantUsersViewModel TenantUsersViewModel { get; set; }
    SubtenantsViewModel SubtenantsViewModel { get; set; }
    string TenantName { get; set; }
}