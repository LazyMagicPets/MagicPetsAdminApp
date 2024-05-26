using Microsoft.AspNetCore.Components;
using System;

namespace BlazorUI;


public class BaseAppJS : IBaseAppJS, IAsyncDisposable
{
    private DotNetObjectReference<BaseAppJS> viewerInstance;
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;
    private readonly NavigationManager navigationManager;

    public BaseAppJS(IJSRuntime jsRuntime, NavigationManager navigationManager) 
    {
        viewerInstance = DotNetObjectReference.Create(this);
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                       "import", "./_content/BlazorUI/js/baseapp.js").AsTask());
        this.navigationManager = navigationManager;
    }

    [JSInvokable]
    public void OnHello(string helloText)
    {
        Console.WriteLine(helloText);
    }

    [JSInvokable]
    public void OnReload()
    {
        Console.WriteLine("BlazorAppJS.OnReload()");
        //navigationManager.NavigateTo("/", true);
        navigationManager.NavigateTo("/", false);
    }

    public virtual async Task SayHello()
        => await (await moduleTask.Value).InvokeVoidAsync("sayHello");
    public virtual async Task Initialize()
        => await (await moduleTask.Value).InvokeVoidAsync("initialize", viewerInstance);
    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
