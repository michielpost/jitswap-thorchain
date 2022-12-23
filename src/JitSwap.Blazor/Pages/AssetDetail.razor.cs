using JitSwap.Blazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace JitSwap.Blazor.Pages
{
    public partial class AssetDetail : MvvmComponentBase<MainViewModel>
    {
        [Parameter]
        public string? Asset { get; set; }

        protected override void OnParametersSet()
        {
            BindingContext.SelectedAsset = Asset;

            base.OnParametersSet();
        }

        protected override async Task LoadDataAsync()
        {
            if (!string.IsNullOrEmpty(Asset))
            {
                //BindingContext.LoadPoolDetail(Asset, Midgard.Period._30d);
                BindingContext.LoadPoolStatsDetail(Asset, Midgard.Period2._30d);

                BindingContext.LoadPoolDepthHistory(Asset, Midgard.Interval.Day, 30, null, null);
                BindingContext.LoadPoolSwapHistory(Asset, Midgard.Interval4.Day, 30, null, null);
                BindingContext.LoadPoolLiquidityHistory(Asset, Midgard.Interval3.Day, 30, null, null);
            }

            await base.LoadDataAsync();
        }
    }
}
