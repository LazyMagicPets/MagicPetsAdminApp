using LazyStack.Client.ViewModels;
using System.Reactive.Disposables;
namespace ViewModels;
/// <summary>
/// Manage user session. This is the root viewModel for the application. It is passed to and accessible 
/// from most components. It keeps track of the current user session. Note that this class is 
/// a singleton created by the DI container but must be initialized before use.
/// 
/// Call the InitAsync() method before calling other methods in the class. This is usually done in the 
/// init.razor OnInitializedAsync() method. 
/// </summary>
public class SessionsViewModel : LzSessionsViewModelAuthNotifications<ISessionViewModel>, ISessionsViewModel
{
    public SessionsViewModel(
        ILzMessages messages,
        ISessionViewModelFactory sessionViewModelFactory,
        ILzHost host,
        HttpClient httpClient,
        ILzClientConfig clientConfig
        )  : base(messages)
    {
        _sessionViewModelFactory = sessionViewModelFactory;
        Host = host;
        HttpClient = httpClient;
        ClientConfig = clientConfig;
    }
    private ISessionViewModelFactory _sessionViewModelFactory;

    public ILzHost Host { get; set; }

    public HttpClient HttpClient { get; set; }   

    public bool ConfigFound { get; set; } = false;
    public bool ConfigError { get; set; } = false;
    public IBaseAppJS? BaseAppJS { get; set; }

    public override Task InitAsync(IOSAccess osAccess, IInternetConnectivitySvc internetConnectivitySvc)
    {
        throw new Exception("Use InitAsync(osAccess, internetConnectivtySvc, baseAppJS) instead");
    }
    // Called from init.razor OnInitializedAsync()
    public async Task InitAsync(IOSAccess osAccess, IInternetConnectivitySvc internetConnectivitySvc, IBaseAppJS baseAppJS)
    {
        BaseAppJS = baseAppJS ?? throw new ArgumentNullException(nameof(baseAppJS));
        await base.InitAsync(
            osAccess: osAccess ?? throw new ArgumentNullException(nameof(osAccess)),
            internetConnectivitySvc ?? throw new ArgumentNullException(nameof(internetConnectivitySvc)));
    }
    public override ISessionViewModel CreateSessionViewModel()
    {
        return _sessionViewModelFactory.Create(OSAccess!, ClientConfig!, InternetConnectivity!);
    }

    // ReadConfigAsync is called from InitAsync() just prior to the IsInitialized being set to true.
    public override async Task ReadConfigAsync()
    {
        try
        {
            await ClientConfig!.ReadAuthConfigAsync(Host.TenancyUrl + "config", "employeeuserpool");
            await ClientConfig!.ReadTenancyConfigAsync(Host.TenancyUrl + "_content/Tenancy/config.json");
            ConfigFound = true;
            ConfigError = false;
            await Messages.SetMessageSetAsync(new LzMessageSet("en-US", LzMessageUnits.Imperial));
        }
        catch (Exception ex)
        {
            ConfigError = true;
            ConfigFound = false;
        }
    }
}
