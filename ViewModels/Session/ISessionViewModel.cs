
using System.ComponentModel;

namespace ViewModels;

public interface ISessionViewModel : ILzSessionViewModelAuthNotifications, INotifyPropertyChanged
{
    IService Store { get; set; }
    UsersViewModel UsersViewModel { get; set; }
    public string TenantName { get; set; }
    public CallerInfo? CallerInfo { get; set; } 
}