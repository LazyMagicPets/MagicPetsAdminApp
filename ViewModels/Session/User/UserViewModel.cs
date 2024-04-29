namespace ViewModels;

[Factory]
public class UserViewModel : LzItemViewModelAuthNotifications<User, UserModel>
{
    public UserViewModel(
        ISessionViewModel sessionViewModel,
        ILzParentViewModel parentViewModel,
        User pet,
        bool? isLoaded = null
        ) : base(sessionViewModel, pet, model: null, isLoaded) 
    {
        _sessionViewModel = sessionViewModel;   
        ParentViewModel = parentViewModel;
        _DTOCreateAsync = sessionViewModel.Store.AddUserAsync;
        _DTOReadIdAsync = sessionViewModel.Store.GetUserByIdAsync;
    }
    private ISessionViewModel _sessionViewModel;
    public override string Id => Data?.Id ?? string.Empty;
    public override long UpdatedAt => Data?.UpdateUtcTick ?? long.MaxValue;

}
