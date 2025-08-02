namespace ViewModels;

using AdminModule;
using LazyMagic.Client.FactoryGenerator; // do not put in global using. Causes runtime error.
[Factory]

public class SubtenantsViewModel : LzItemsViewModelAuthNotifications<SubtenantViewModel, Subtenant, SubtenantModel>
{
    public SubtenantsViewModel(
        [FactoryInject] ILoggerFactory loggerFactory,
        [FactoryInject] IAdminModuleClient adminModuleClient,
        [FactoryInject] ISubtenantViewModelFactory subtenantViewModelFactory) : base(loggerFactory)
    {
        SubtenantViewModelFactory = subtenantViewModelFactory;
        _DTOReadListAsync = adminModuleClient.AdminModuleListSubtenantsAsync;
    }
    private ISessionViewModel _sessionViewModel;
    public ISubtenantViewModelFactory? SubtenantViewModelFactory { get; init; }
    public override (SubtenantViewModel, string) NewViewModel(Subtenant dto)
        => (SubtenantViewModelFactory!.Create(this, dto), string.Empty);

}


