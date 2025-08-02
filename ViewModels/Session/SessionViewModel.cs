using AdminApi;
using Amazon;
using LazyMagic.Client.FactoryGenerator; // do not put in global using. Causes runtime error.

namespace ViewModels;
/// <summary>
/// The SessionViewModel is the root viewModel for a tenantuser session.
/// This class maintains the "state" of the tenantuser session, which includes 
/// the data (in this case the PetsViewMode).
/// </summary>
[Factory]
public class SessionViewModel : BaseAppSessionViewModel, ISessionViewModel, ICurrentSessionViewModel
{
    public SessionViewModel(
        [FactoryInject] ILoggerFactory loggerFactory, // singleton
        [FactoryInject] ILzClientConfig clientConfig, // singleton
        [FactoryInject] IConnectivityService connectivityService, // singleton
        [FactoryInject] ILzHost lzHost, // singleton
        [FactoryInject] ILzMessages messages, // singleton
        [FactoryInject] IAuthProcess authProcess, // transient
        [FactoryInject] ITenantUsersViewModelFactory tenantusersViewModelFactory, // transient  
        [FactoryInject] ISubtenantsViewModelFactory subtenantsViewModelFactory, // singleton
        [FactoryInject] IPetsViewModelFactory petsViewModelFactory, // transient
        [FactoryInject] ICategoriesViewModelFactory categoriesViewModelFactory, // transient
        [FactoryInject] ITagsViewModelFactory tagsViewModelFactory, // transient
        ISessionsViewModel sessionsViewModel
        )
        : base(loggerFactory, authProcess, clientConfig, connectivityService, messages,
            petsViewModelFactory, categoriesViewModelFactory, tagsViewModelFactory)  
    {
        try
        {
            var tenantKey = (string?)clientConfig.TenancyConfig["tenantKey"] ?? throw new Exception("Cognito TenancyConfig.tenantKey is null");
            TenantName = AppConfig.TenantName;
            authProcess.SetAuthenticator(clientConfig.AuthConfigs?["TenantAuth"]!);
            authProcess.SetSignUpAllowed(false);

            this.tenantusersViewModelFactory = tenantusersViewModelFactory ?? throw new ArgumentNullException(nameof(tenantusersViewModelFactory));
            TenantUsersViewModel = tenantusersViewModelFactory.Create();

            this.subtenantsViewModelFactory = subtenantsViewModelFactory ?? throw new ArgumentNullException(nameof(subtenantsViewModelFactory));
            SubtenantsViewModel = subtenantsViewModelFactory.Create();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"SetAuthenticator failed. {ex.Message}");
            throw new Exception("oops");
        }
    }

    private ITenantUsersViewModelFactory tenantusersViewModelFactory;
    private ISubtenantsViewModelFactory subtenantsViewModelFactory;

    public TenantUsersViewModel TenantUsersViewModel { get; set; }  
    public SubtenantsViewModel SubtenantsViewModel { get; set; }
    public string TenantName { get; set; } = string.Empty;

    // Base class calls LoadAsync () when IsSignedIn changes to true
    public override Task LoadAsync()
    {
        // don't do preloadading. Instead, load sections, TenantUsers, when
        // the user navigates to them. 
        return Task.CompletedTask;
    }
    // Base class calls UnloadAsync () when IsSignedIn changes to false
    public override async Task UnloadAsync()
    {
        if(TenantUsersViewModel != null)
           TenantUsersViewModel = tenantusersViewModelFactory.Create();

        if(SubtenantsViewModel != null)
            SubtenantsViewModel = subtenantsViewModelFactory.Create();

        await Task.Delay(0);    
    }
}