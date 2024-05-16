namespace ViewModels;

public interface ISessionsViewModel : ILzSessionsViewModelAuthNotifications<ISessionViewModel> {
    public bool ConfigFound { get; set; }
    public bool ConfigError { get; set; }
}

