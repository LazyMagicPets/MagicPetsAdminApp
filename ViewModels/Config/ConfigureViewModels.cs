using ConsumerModule;
using LazyMagic.Shared;
using PublicModule;
using StoreModule;

namespace ViewModels;

public static class ConfigureViewModels
{
    public static IServiceCollection AddAppViewModels(this IServiceCollection services)
    {

        ViewModelsRegisterFactories.ViewModelsRegister(services); // Register Factory Classes

        // ISessionsViewModel serves as the global app state. It manages multiple 
        // sessions, each represented by a SessionViewModel. This is useful for 
        // POS terminal apps where multiple sessions can be active at once. PWAs 
        // and mobile apps are generally single session apps so there will be only
        // a single SessionViewModel for those app types.
        services.AddSingleton<ISessionsViewModel, SessionsViewModel>();

        // Register the SessionsViewModel as the base app sessions view model for BaseApp.ViewModel library use.
        services.AddSingleton<IBaseAppSessionsViewModel<ISessionViewModel>>(provider => provider!.GetService<ISessionsViewModel>()!);
        services.AddSingleton<IBaseAppSessionsViewModelBase>(provider => provider!.GetService<ISessionsViewModel>()!);

        // Register the ClientSDK 
        services.AddSingleton<IAdminApi>(serviceProvider =>
        {
            // Get the SessionsViewModel from the service provider and then 
            // return the HostApi from it.
            var lzHttpClient = serviceProvider.GetRequiredService<ILzHttpClient>();
            var adminApi = new AdminApi.AdminApi(lzHttpClient);
            return adminApi;
        });

        // Register the modules used from the Client SDK.
        services.AddSingleton<IAdminModuleClient>(provider => provider.GetRequiredService<IAdminApi>());
        services.AddSingleton<IPublicModuleClient>(provider => provider.GetRequiredService<IAdminApi>());
        services.AddSingleton<IConsumerModuleClient>(provider => provider.GetRequiredService<IAdminApi>());
        services.AddSingleton<IStoreModuleClient>(provider => provider.GetRequiredService<IAdminApi>());


        // Register the ICurrentSessionViewModel to return the current session view model.
        // Use the ICurrentSessionViewModel interface in most page components. Generally, the 
        // SessionViewModel is where properties reference data used in the current session.
        services.AddTransient<ICurrentSessionViewModel>(provider =>
        {
            // Get the SessionsViewModel from the service provider and then 
            // return the SessionViewModel from it.
            var sessionsViewModel = provider.GetRequiredService<ISessionsViewModel>();
            var currentSession = sessionsViewModel.SessionViewModel as ICurrentSessionViewModel;
            return currentSession!;
        });

        services.AddBaseAppViewModels();

        return services;
    }
}

