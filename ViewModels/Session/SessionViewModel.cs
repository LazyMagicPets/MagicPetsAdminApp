using Amazon;

namespace ViewModels;
/// <summary>
/// The SessionViewModel is the root viewModel for a user session.
/// This class maintains the "state" of the use session, which includes 
/// the data (in this case the PetsViewMode).
/// </summary>
[Factory]
public class SessionViewModel : LzSessionViewModelAuthNotifications, ISessionViewModel, ILzTransient
{
    public SessionViewModel(
        IOSAccess osAccess, // singleton
        ILzClientConfig clientConfig, // singleton
        IInternetConnectivitySvc internetConnectivity, // singleton
        [FactoryInject] ILzHost lzHost, // singleton
        [FactoryInject] ILzMessages messages, // singleton
        [FactoryInject] IAuthProcess authProcess, // transient
        [FactoryInject] IMethodMapWrapper methodMap, // singleton
        [FactoryInject] IUsersViewModelFactory usersViewModelFactory // transient
        )
        : base( authProcess, osAccess, clientConfig, internetConnectivity, messages)  
    {

        ILzHttpClient httpClient = new LzHttpClient(clientConfig, methodMap, authProcess.AuthProvider, lzHost);
        Store = new Service(httpClient);
        NotificationsSvc = new StoreNotificationSvc(this, clientConfig, lzHost, internetConnectivity);
        this.usersViewModelFactory = usersViewModelFactory ?? throw new ArgumentNullException(nameof(usersViewModelFactory));
        UsersViewModel = usersViewModelFactory.Create(this);
        try
        {
            //var _region = (string?)clientConfig.AuthConfig["awsRegion"] ?? throw new Exception("Cognito AuthConfig.region is null");
            //var regionEndpoint = RegionEndpoint.GetBySystemName(_region);
            authProcess.SetAuthenticator(clientConfig.AuthConfig);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SetAuthenticator failed. {ex.Message}");
            throw new Exception("opps");
        }
    }
    public IService Store { get; set; }
    private IUsersViewModelFactory usersViewModelFactory;
    public UsersViewModel UsersViewModel { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public CallerInfo? CallerInfo { get; set; }

    // Base class calls LoadAsync () when IsSignedIn changes to true
    public override async Task LoadAsync()
    {
        try
        {
            var callerInfoJson = await Store.CallerInfoAsync();
            if (callerInfoJson != null)
                CallerInfo = JsonConvert.DeserializeObject<CallerInfo>(callerInfoJson);
        } catch (Exception ex)
        {
            Console.WriteLine($"CallerInfoAsync failed. {ex.Message}");
        }   

        await UsersViewModel.ReadAsync();
    }
    // Base class calls UnloadAsync () when IsSignedIn changes to false
    public override async Task UnloadAsync()
    {
        UsersViewModel = usersViewModelFactory.Create(this);
        await Task.Delay(0);    
    }
}