namespace ViewModels;

public interface ISessionsViewModel : ILzSessionsViewModelAuthNotifications<ISessionViewModel> {
    public bool ConfigFound { get; set; }
    public bool ConfigError { get; set; }
    public IBaseAppJS? BaseAppJS { get; set; }
    Task InitAsync(IOSAccess osAccess, IInternetConnectivitySvc internetConnectivitySvc, IBaseAppJS baseAppJS);
}

