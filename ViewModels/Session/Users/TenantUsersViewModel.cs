
namespace ViewModels;
using LazyMagic.Client.FactoryGenerator; // do not put in global using. Causes runtime error.
[Factory]

public class TenantUsersViewModel : LzItemsViewModelAuthNotifications<TenantUserViewModel, TenantUser, TenantUserModel>
{
    public TenantUsersViewModel(
        [FactoryInject] ILoggerFactory loggerFactory,
        ISessionViewModel sessionViewModel,
        [FactoryInject] ITenantUserViewModelFactory tenantuserViewModelFactory) : base(loggerFactory, sessionViewModel)
    {
        _sessionViewModel = sessionViewModel;
        TenantUserViewModelFactory = tenantuserViewModelFactory;
        _DTOReadListAsync = sessionViewModel.Admin.ListTenantUsersAsync;
    }
    private ISessionViewModel _sessionViewModel;
    public ITenantUserViewModelFactory? TenantUserViewModelFactory { get; init; }

    public override (TenantUserViewModel, string) NewViewModel(TenantUser dto)
        => (TenantUserViewModelFactory!.Create(_sessionViewModel, this, dto), string.Empty);


    public override async Task<(bool, string)> ReadAsync(bool forceload = false)
    => await base.ReadAsync(forceload);
}
