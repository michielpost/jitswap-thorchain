using JitSwap.Blazor;
using JitSwap.Blazor.Services;
using JitSwap.Blazor.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Reflection;

namespace JitSwap.Blazor
{
    public class Program
    {
        public static string? Version { get; set; }

        public static string? GetVersionHash()
        {
            if (Version != null)
            {
                int sep = Version.LastIndexOf('-');
                if (sep >= 0 && sep < Version.Length)
                    return Version.Substring(sep + 1);
            }
            return null;
        }

        public static string? GetVersionWithoutHash()
        {
            if (Version != null)
            {
                int sep = Version.LastIndexOf('-');
                if (sep >= 0)
                    return Version.Substring(0, sep);
            }
            return Version;
        }

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddMudServices();

            //Set Version
            Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            //Services
            builder.Services.AddSingleton<DataService>();

            //Register ViewModels
            builder.Services.AddSingleton<MainViewModel>();

            await builder.Build().RunAsync();
        }
    }
}