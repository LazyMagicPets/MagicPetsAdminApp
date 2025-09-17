namespace ViewModels;

using AdminModule;
using LazyMagic.Client.FactoryGenerator; // do not put in global using. Causes runtime error.
[Factory]

public class SubtenantViewModel : LzItemViewModel<Subtenant, SubtenantModel>
{
    public SubtenantViewModel(
        [FactoryInject] ILoggerFactory loggerFactory,
        [FactoryInject] IAdminModuleClient adminModuleClient,
        ILzParentViewModel parentViewModel,
        Subtenant subtenant,
        bool? isLoaded = null
        ) : base(loggerFactory, subtenant, model: null, isLoaded)
    {
        ParentViewModel = parentViewModel;
        _DTOCreateAsync = adminModuleClient.AdminModuleAddSubtenantAsync;
        _DTOReadAsync = adminModuleClient.AdminModuleGetSubtenantByIdAsync;
        _DTOUpdateAsync = adminModuleClient.AdminModuleUpdateSubtenantAsync;
        _DTODeleteAsync = adminModuleClient.AdminModuleDeleteSubtenantAsync;
    }
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
