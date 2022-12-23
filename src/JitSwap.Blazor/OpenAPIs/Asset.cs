using System.Text.Json.Serialization;

namespace JitSwap.Blazor.OpenAPIs
{
    public class Asset
    {
        /// <summary>
        /// Chain type
        /// </summary>
        [JsonPropertyName("chain")]
        public virtual Chain Chain { get; set; }
        /// <summary>
        /// Symbol of Asset
        /// </summary>
        [JsonPropertyName("symbol")]
        public virtual string Symbol { get; set; } = default!;
        /// <summary>
        /// Ticker of asset
        /// </summary>
        [JsonPropertyName("ticker")]
        public virtual string Ticker { get; set; } = default!;

        public string Short => $"{Ticker}.{Chain}";
    }
}
