namespace ViewModels;
using LazyMagic.Client.FactoryGenerator; // do not put in global using. Causes runtime error.
[Factory]

public class SubtenantsViewModel : LzItemsViewModelAuthNotifications<SubtenantViewModel, Subtenant, SubtenantModel>
{
    public SubtenantsViewModel(
        [FactoryInject] ILoggerFactory loggerFactory,
        ISessionViewModel sessionViewModel,
        [FactoryInject] ISubtenantViewModelFactory subtenantViewModelFactory) : base(loggerFactory, sessionViewModel)
    {
        _sessionViewModel = sessionViewModel;
        SubtenantViewModelFactory = subtenantViewModelFactory;
        _DTOReadListAsync = sessionViewModel.Admin.ListSubtenantsAsync;
    }
    private ISessionViewModel _sessionViewModel;
    public ISubtenantViewModelFactory? SubtenantViewModelFactory { get; init; }
    public override (SubtenantViewModel, string) NewViewModel(Subtenant dto)
        => (SubtenantViewModelFactory!.Create(_sessionViewModel, this, dto), string.Empty);

}


