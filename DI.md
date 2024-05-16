# Dependency Injection Registrations

We register services in a hierachael manner.  The following is a list of the services that are registered in the `WASMApp.Program` and `MAUIApp.MauiProgram` classes.  The `WASMApp.Program` class is the entry point for the Blazor WebAssembly application.  The `MAUIApp.MauiProgram` class is the entry point for the .NET MAUI application.

In addition, we show the registrations peformed by the LazyStack libraries used. Note that most classes registered by LazyStack use the `TryAdd` methods to avoid overwriting existing registrations.  This allows the developer to override the default registration if needed.

```
MAUIApp.MauiProgram, WASMApp.Program
	.AddSingleton<HttpClient>(...)
	.AddSingleton<ILzHost>(...) 
	.AddBlazorUI() // BlazorUI.ConfigureBlazorUI
		.AddMudServices()
		ILzMessages messages = new LzMessages();
		.AddSingleton<ILzMessages>(messages) 
		.AddAppViewModels() // ViewModels.ConfigureViewModels
			LzViewModelFactory.RegisterLz(services, assembly!)
			.AddSingleton<ILzClientConfig,LzClientConfig>() 
			.AddLazyStackAuthCognito() // LazyStack.Client.Auth.ConfigureLazyStackAuthCognito
				.TryAddTransient<ILzHttpClient, LzHttpClient>()
				.TryAddTransient<IAuthProvider, AuthProviderCognito>()
				.AddLazyStackAuth()  // LazyStack.Client.Auth.ConfigureLazyStackAuth
					.TryAddTransient<IAuthProcess, AuthProcess>();
					.TryAddTransient<ILoginFormat, LoginFormat>();
					.TryAddTransient<IEmailFormat, EmailFormat>();
					.TryAddTransient<IPhoneFormat, PhoneFormat>();
					.TryAddTransient<ICodeFormat, CodeFormat>();
					.TryAddTransient<IPasswordFormat, PasswordFormat>();
					.TryAddSingleton<ILzHost, LzHost>();				
			.AddSingleton<ISessionsViewModel, SessionsViewModel>() 
			.TryAddSingleton(methodMapWrapper!) 
			.TryAddTransient<IService, Service>() 
			.TryAddTransient<ILzNotificationSvc, StoreNotificationSvc>() 
```

The LzViewModelFactory.RegisterLz() method uses reflection to register any classes that implement one of these interfaces:
- ITransient
- ISingleton
- IScoped 

Using reflection to register classes is no longer a recommended pattern in .NET because of tree shaking. Alternative patterns are being considered for future versions of LazyStack.