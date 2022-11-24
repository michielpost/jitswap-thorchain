using JitSwap.Blazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace JitSwap.Blazor.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public MainViewModel BindingContext { get; set; } = default!;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            BindingContext.MidgardUrl = "https://midgard.ninerealms.com";

        }

    }
}
