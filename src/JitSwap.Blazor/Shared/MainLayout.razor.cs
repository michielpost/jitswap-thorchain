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

            BindingContext.PropertyChanged += BindingContext_PropertyChanged;
        }

        private void BindingContext_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(BindingContext.MidgardUrl))
            {
                this.StateHasChanged();
            }
        }
    }
}
