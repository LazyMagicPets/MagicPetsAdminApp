namespace WASMApp;
public partial class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<Main>("#main");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        // We use the launchSettings.json profile ASPNETCORE_ENVIRONMENT environment variable
        // to determine the host addresses for the API host and Tenancy host.
        //
        // Examples:
        // Production: "ASPNETCORE_ENVIRONMENT": "Production" 
        //  The API and Tenancy host are the same and are the base address of the cloudfront distribution
        //  the app is loaded from.
        //
        // Debug against LocalHost API:
        //  "ASPNETCORE_ENVIRONMENT": "https://localhost:5001,https://admin.lazymagicdev.click"
        //  The first url is the API host, the second is the tenancy host.
        //
        // Debug against CloundFront deployment:
        //   "ASPNETCORE_ENVIRONMENT": "https://admin.lazymagicdev.click"
        //   The url is the API host and the tenancy host.


        var hostEnvironment = builder.HostEnvironment;
        var apiUrl = string.Empty;
        var tenancyUrl = string.Empty;
        //var wsUrl = "wss://2v24ejuki8.execute-api.us-west-2.amazonaws.com/Dev/";
        var wsUrl = string.Empty;   
        var isLocal = false; // Is the code being served from a local development host?
        switch(hostEnvironment.Environment)
        {
            case "Production":
                Console.WriteLine("Loaded from CloudFront");
                apiUrl = tenancyUrl = builder.HostEnvironment.BaseAddress;
                // Note: Even though we set wsUrl here, it may be overridden
                // when we call /config. This is necessary because cloudfront 
                // may be configured with the websocket url as an origin or 
                // we may choose to call it directly.
                wsUrl = tenancyUrl.Replace("https", "wss").Replace("http", "ws") + "ws";
                break;
            default:

                Console.WriteLine("Development environment, loaded from localhost");
                var envVar = hostEnvironment.Environment;
                // The variable is a list of urls, separated by a comma.
                var urls = envVar.Split(',');
                apiUrl = urls[0];
                apiUrl = apiUrl.EndsWith('/') ? apiUrl : apiUrl + '/';
                tenancyUrl = urls.Length > 1 ? urls[1] : urls[0];
                tenancyUrl = tenancyUrl.EndsWith('/') ? tenancyUrl : tenancyUrl + '/';
                wsUrl = tenancyUrl.Replace("https", "wss").Replace("http", "ws")  + "ws";   
                Console.WriteLine($"apiUrl: {apiUrl}");
                Console.WriteLine($"tenancyUrl: {tenancyUrl}");
                isLocal = true; 
                break;
        }

        builder.Services
            .AddSingleton(sp => new HttpClient { BaseAddress = new Uri(apiUrl) })
            .AddSingleton<ILzHost>(sp => new LzHost(
                url: apiUrl,  // api url
                tenancyUrl: tenancyUrl, // tenancy assets url
                wsUrl: wsUrl, // web socket url
                isMAUI: false, // sets isWASM to true
                isAndroid: false,
                isLocal: isLocal))
            .AddBlazorUI(); // See Config/ConfigureViewModels.cs
            

        await builder.Build().RunAsync();
    }
}