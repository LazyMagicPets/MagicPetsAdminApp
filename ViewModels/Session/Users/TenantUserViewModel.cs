
namespace ViewModels;
using LazyMagic.Client.FactoryGenerator; // do not put in global using. Causes runtime error.   
[Factory]
public class TenantUserViewModel : LzItemViewModelAuthNotifications<TenantUser, TenantUserModel>
{
    public TenantUserViewModel(
        [FactoryInject] ILoggerFactory loggerFactory,
        ISessionViewModel sessionViewModel,
        ILzParentViewModel parentViewModel,
        TenantUser tenantuser,
        bool? isLoaded = null
        ) : base(loggerFactory, sessionViewModel, tenantuser, model: null, isLoaded)
    {
        _sessionViewModel = sessionViewModel;
        ParentViewModel = parentViewModel;
        _DTOCreateAsync = sessionViewModel.Admin.AddTenantUserAsync;
        _DTOReadAsync = sessionViewModel.Admin.GetTenantUserByIdAsync;
        _DTOUpdateAsync = sessionViewModel.Admin.UpdateTenantUserAsync;
        _DTODeleteAsync = sessionViewModel.Admin.DeleteTenantUserAsync;
    }
    private ISessionViewModel _sessionViewModel;
    public override string Id => Data?.Id ?? string.Empty;
    public override long UpdatedAt => Data?.UpdateUtcTick ?? long.MaxValue;
}

