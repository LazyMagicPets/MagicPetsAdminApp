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
public class SessionViewModel : LzSessionViewModelAuthNotifications, ISessionViewModel
{
    public SessionViewModel(
        [FactoryInject] ILoggerFactory loggerFactory, // singleton
        [FactoryInject] ILzClientConfig clientConfig, // singleton
        [FactoryInject] IInternetConnectivitySvc internetConnectivity, // singleton
        [FactoryInject] ILzHost lzHost, // singleton
        [FactoryInject] ILzMessages messages, // singleton
        [FactoryInject] IAuthProcess authProcess, // transient
        [FactoryInject] ITenantUsersViewModelFactory tenantusersViewModelFactory, // transient  
        [FactoryInject] ISubtenantsViewModelFactory subtenantsViewModelFactory // singleton
        )
        : base(loggerFactory, authProcess, clientConfig, internetConnectivity, messages)  
    {
        try
        {
            var tenantKey = (string?)clientConfig.TenancyConfig["tenantKey"] ?? throw new Exception("Cognito TenancyConfig.tenantKey is null");
            TenantName = AppConfig.TenantName;
            authProcess.SetAuthenticator(clientConfig.AuthConfigs?["TenantAuth"]!);
            authProcess.SetSignUpAllowed(false);


            var sessionId = Guid.NewGuid().ToString(); 

            ILzHttpClient httpClientAdmin = new LzHttpClient(loggerFactory, authProcess.AuthProvider, lzHost, sessionId);
            Admin = new AdminApi.AdminApi(httpClientAdmin);

            this.tenantusersViewModelFactory = tenantusersViewModelFactory ?? throw new ArgumentNullException(nameof(tenantusersViewModelFactory));
            TenantUsersViewModel = tenantusersViewModelFactory.Create(this);

            this.subtenantsViewModelFactory = subtenantsViewModelFactory ?? throw new ArgumentNullException(nameof(subtenantsViewModelFactory));
            SubtenantsViewModel = subtenantsViewModelFactory.Create(this);

        }
        catch (Exception ex)
        {
            Console.WriteLine($"SetAuthenticator failed. {ex.Message}");
            throw new Exception("oops");
        }
    }
    public IAdminApi Admin { get; set; }
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
           TenantUsersViewModel = tenantusersViewModelFactory.Create(this);

        if(SubtenantsViewModel != null)
            SubtenantsViewModel = subtenantsViewModelFactory.Create(this);

        await Task.Delay(0);    
    }
}