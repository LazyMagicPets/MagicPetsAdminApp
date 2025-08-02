
namespace ViewModels;

using AdminModule;
using LazyMagic.Client.FactoryGenerator; // do not put in global using. Causes runtime error.
[Factory]

public class TenantUsersViewModel : LzItemsViewModelAuthNotifications<TenantUserViewModel, TenantUser, TenantUserModel>
{
    public TenantUsersViewModel(
        [FactoryInject] ILoggerFactory loggerFactory,
        [FactoryInject] IAdminModuleClient adminModuleClient,
        [FactoryInject] ITenantUserViewModelFactory tenantuserViewModelFactory) : base(loggerFactory)
    {
        TenantUserViewModelFactory = tenantuserViewModelFactory;
        _DTOReadListAsync = adminModuleClient.AdminModuleListTenantUsersAsync;
    }

    public ITenantUserViewModelFactory? TenantUserViewModelFactory { get; init; }

    public override (TenantUserViewModel, string) NewViewModel(TenantUser dto)
        => (TenantUserViewModelFactory!.Create(this, dto), string.Empty);


    public override async Task<(bool, string)> ReadAsync(bool forceload = false)
    => await base.ReadAsync(forceload);
}
