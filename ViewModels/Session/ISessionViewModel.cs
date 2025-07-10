
using AdminApi;
using System.ComponentModel;

namespace ViewModels;

public interface ISessionViewModel : IBaseAppSessionViewModelAuthNotifications
{
    IAdminApi Admin { get; set; }
    IStoreApi Store { get; set; }
    IConsumerApi Consumer { get; set; }
    IPublicApi Public { get; set; }
    TenantUsersViewModel TenantUsersViewModel { get; set; }
    SubtenantsViewModel SubtenantsViewModel { get; set; }
    string TenantName { get; set; }
}