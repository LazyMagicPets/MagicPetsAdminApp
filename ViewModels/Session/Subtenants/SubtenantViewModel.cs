namespace ViewModels;
using LazyMagic.Client.FactoryGenerator; // do not put in global using. Causes runtime error.
[Factory]

public class SubtenantViewModel : LzItemViewModelAuthNotifications<Subtenant, SubtenantModel>
{
    public SubtenantViewModel(
        [FactoryInject] ILoggerFactory loggerFactory,
        ISessionViewModel sessionViewModel,
        ILzParentViewModel parentViewModel,
        Subtenant subtenant,
        bool? isLoaded = null
        ) : base(loggerFactory, sessionViewModel, subtenant, model: null, isLoaded)
    {
        _sessionViewModel = sessionViewModel;
        ParentViewModel = parentViewModel;
        _DTOCreateAsync = sessionViewModel.Admin.AddSubtenantAsync;
        _DTOReadAsync = sessionViewModel.Admin.GetSubtenantByIdAsync;
        _DTOUpdateAsync = sessionViewModel.Admin.UpdateSubtenantAsync;
        _DTODeleteAsync = sessionViewModel.Admin.DeleteSubtenantAsync;
    }
    private ISessionViewModel _sessionViewModel;
    public override string Id => Data?.Id ?? string.Empty;
    public override long UpdatedAt => Data?.UpdateUtcTick ?? long.MaxValue;

    /// <summary>
    /// Placeholder while we transition to managing the subtenants 
    /// from Lambdas. Todo: update when we allow the backend to create subtenants
    /// </summary>
    /// <returns></returns>
    public static Subtenant NewSubtenant()
    {
        return new Subtenant
        {
        };      
    }

}
