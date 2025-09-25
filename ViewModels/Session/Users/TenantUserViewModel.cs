
namespace ViewModels;

using AdminModule;
using LazyMagic.Client.FactoryGenerator; // do not put in global using. Causes runtime error.   
[Factory]
public class TenantUserViewModel : LzItemViewModel<TenantUser, TenantUserModel>
{
    public TenantUserViewModel(
        [FactoryInject] ILoggerFactory loggerFactory,
        [FactoryInject] IAdminModuleClient adminModuleClient,
        ILzParentViewModel parentViewModel,
        TenantUser tenantuser,
        bool? isLoaded = null
        ) : base(loggerFactory, tenantuser, model: null, isLoaded)
    {
        ParentViewModel = parentViewModel;
        _DTOCreateAsync = adminModuleClient.AdminModuleAddTenantUserAsync;
        _DTOReadAsync = adminModuleClient.AdminModuleGetTenantUserByIdAsync;
        _DTOUpdateAsync = adminModuleClient.AdminModuleUpdateTenantUserAsync;
        _DTODeleteAsync = adminModuleClient.AdminModuleDeleteTenantUserAsync;
    }
    public override string Id => Data?.Id ?? string.Empty;
    public override long UpdatedAt => Data?.UpdateUtcTick ?? long.MaxValue;
}

