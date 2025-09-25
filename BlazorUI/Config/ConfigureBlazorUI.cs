

using MudBlazor.Services;

namespace BlazorUI;

public static class ConfigureBlazorUI
{
    public static IServiceCollection AddBlazorUI(this IServiceCollection services)
    {
        services.AddBaseAppBlazorUI();
        return services;
    }
    public static ILzMessages AddBlazorUIMessages(this ILzMessages lzMessages)
    {
        lzMessages.AddBaseAppMessages(); // Add the BaseApp messages    

        List<string> messages = [
            "system/{culture}/AdminApp/Messages.json",
            ];
        lzMessages.MessageFiles.AddRange(messages);
        return lzMessages;
    }
}
