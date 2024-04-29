namespace ViewModels;
[Factory]
public class UsersViewModel : LzItemsViewModelAuthNotifications<UserViewModel, User, UserModel>
{
    public UsersViewModel(
        ISessionViewModel sessionViewModel,
        [FactoryInject] IUserViewModelFactory userViewModelFactory) : base(sessionViewModel)  
    { 
        _sessionViewModel = sessionViewModel;
        UserViewModelFactory = userViewModelFactory;
        _DTOReadListAsync = sessionViewModel.Store.ListUsersAsync;
        SvcTestAsync = sessionViewModel.Store.TestAsync;

    }
    private ISessionViewModel _sessionViewModel;
    public IUserViewModelFactory? UserViewModelFactory { get; init; }
    public override (UserViewModel, string) NewViewModel(User dto)
        => (UserViewModelFactory!.Create(_sessionViewModel, this, dto), string.Empty);
    public Func<Task<string>>? SvcTestAsync { get; init; }
    public async Task<string> TestAsync()
    {
        if (SvcTestAsync is null)
            return string.Empty;
        return await SvcTestAsync();
    }

    public override async Task<(bool, string)> ReadAsync(bool forceload = false, StorageAPI storageAPI = StorageAPI.DTO)
    => await base.ReadAsync(string.Empty, forceload, storageAPI);

    public async Task<(bool, string)> CreateAsync()
    {
        var userMsg = "Can't Create User";
        await Task.Delay(0);
        try
        {
            var user = new User();
            //user.Id = Guid.NewGuid().ToString();
            var userViewModel = UserViewModelFactory!.Create(_sessionViewModel, this, user);
            userViewModel.State = LzItemViewModelState.New;
            CurrentViewModel = userViewModel;
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, Log(userMsg, ex.Message));
        }
    }
}
