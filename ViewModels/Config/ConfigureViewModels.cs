using ConsumerModule;
using LazyMagic.Shared;
using PublicModule;
using StoreModule;

namespace ViewModels;

public static class ConfigureViewModels
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {

        ViewModelsRegisterFactories.ViewModelsRegister(services); // Register Factory Classes


        // Register the ClientSDK 
        services.AddScoped<IAdminApi>(serviceProvider =>
        {
            var lzHost = serviceProvider.GetRequiredService<ILzHost>();
            var authenticationHandler = serviceProvider.GetRequiredService<IAuthenticationHandler>();
            var handler = authenticationHandler.CreateHandler();
            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(lzHost.GetApiUrl("")) // LocalApiUrl or RemoteApiUrl depending on UseLocalhostApi property
            };
            var api = new AdminApi.AdminApi(httpClient);
            return api;
        });

        // Register the modules used from the Client SDK.
        services.AddScoped<IAdminModuleClient>(provider => provider.GetRequiredService<IAdminApi>());
        services.AddScoped<IPublicModuleClient>(provider => provider.GetRequiredService<IAdminApi>());
        services.AddScoped<IConsumerModuleClient>(provider => provider.GetRequiredService<IAdminApi>());
        services.AddScoped<IStoreModuleClient>(provider => provider.GetRequiredService<IAdminApi>());

        services.AddScoped<ISessionViewModel, SessionViewModel>();
        services.AddTransient<IBaseAppSessionViewModel>(sp => sp.GetRequiredService<ISessionViewModel>());

        services.AddBaseAppViewModels();

        return services;
    }
}

