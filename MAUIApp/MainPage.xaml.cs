namespace MAUIApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
#if WINDOWS && DEBUG
            // Automated-UI-test hook: expose the WebView2 Chrome DevTools Protocol endpoint so
            // tests can attach with Playwright connectOverCDP('http://localhost:9222') and drive
            // the Blazor Hybrid DOM. Reliable for packaged + unpackaged runs (the
            // WEBVIEW2_ADDITIONAL_BROWSER_ARGUMENTS env var is MSIX-sandboxed). DEBUG + Windows only.
            blazorWebView.BlazorWebViewInitializing += (s, e) =>
            {
                e.EnvironmentOptions ??= new Microsoft.Web.WebView2.Core.CoreWebView2EnvironmentOptions();
                var existing = e.EnvironmentOptions.AdditionalBrowserArguments ?? string.Empty;
                if (!existing.Contains("--remote-debugging-port"))
                    e.EnvironmentOptions.AdditionalBrowserArguments =
                        (existing + " --remote-debugging-port=9222").Trim();
            };
#endif
        }
    }
}
