using JitSwap.Blazor.OpenAPIs;

namespace Midgard
{
    public partial class PoolStatsDetail
    {
        public Asset? AssetTyped => Utils.AssetFromString(this.Asset);
    }
}
