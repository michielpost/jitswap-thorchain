using JitSwap.Blazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace JitSwap.Blazor.Pages
{
    public partial class AssetDetail : MvvmComponentBase<MainViewModel>
    {
        [Parameter]
        public string? Asset { get; set; }


    }
}
