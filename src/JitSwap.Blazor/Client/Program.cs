using Blazored.LocalStorage;
using JitSwap.Blazor;
using JitSwap.Blazor.Services;
using JitSwap.Blazor.ViewModels;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Reflection;
using webvNext.DataLoader.Cache;

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

            ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress);

            await builder.Build().RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, string baseAddress)
        {
            services.AddMudServices();

            //Set Version
            Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

            //Services
            services.AddScoped<DataService>();
            services.AddScoped<StorageService>();

            services.AddSingleton<MemoryDataCache>();


            //Register ViewModels
            services.AddScoped<MainViewModel>();

            services.AddBlazoredLocalStorage();
        }
    }
}