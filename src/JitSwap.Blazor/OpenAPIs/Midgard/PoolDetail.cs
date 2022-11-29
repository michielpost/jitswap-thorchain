using JitSwap.Blazor.OpenAPIs;

namespace Midgard
{
    public partial class PoolDetail
    {
        public Asset? AssetTyped => Utils.AssetFromString(this.Asset);
    }
}
