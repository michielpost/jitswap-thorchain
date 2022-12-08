using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JitSwap.Blazor.Services;
using JitSwap.Blazor.Shared.Components;
using Midgard;
using webvNext.DataLoader;
using webvNext.DataLoader.Cache;

namespace JitSwap.Blazor.ViewModels
{
    public partial class MainViewModel : ObservableRecipient
    {
        private readonly DataService dataService;
        private readonly StorageService storageService;
        private readonly MemoryDataCache memoryDataCache;

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private string? midgardUrl;

        [ObservableProperty]
        private string? selectedAsset;

        public DataLoaderViewModel<Dictionary<string, string>> KnownPoolsListVM { get; set; } = new();

        public DataLoaderViewModel<List<PoolDetail>> PoolsListVM { get; set; } = new();

        public DataLoaderViewModel<Health> HealthVM { get; set; } = new();

        public DataLoaderViewModel<Network> NetworkDataVM { get; set; } = new();

        public DataLoaderViewModel<StatsData> StatsDataVM { get; set; } = new();

        public DataLoaderViewModel<List<ChurnItem>> ChurnListVM { get; set; } = new();

        public DataLoaderViewModel<List<Node>> NodesListVM { get; set; } = new();

        public DataLoaderViewModel<PoolDetail> PoolDetailVM { get; set; } = new();

        public DataLoaderViewModel<PoolStatsDetail> PoolStatsDetailVM { get; set; } = new();

        public DataLoaderViewModel<DepthHistory> PoolDepthHistoryVM { get; set; } = new();

        public DataLoaderViewModel<EarningsHistory> EarningsHistoryVM { get; set; } = new();

        public DataLoaderViewModel<SwapHistory> PoolSwapHistoryVM { get; set; } = new();

        public DataLoaderViewModel<TVLHistory> TotalValueLockedHistoryVM { get; set; } = new();

        public DataLoaderViewModel<LiquidityHistory> PoolLiquidityHistoryVM { get; set; } = new();


        //TODO:
        //Actions List (optional? address)

        //Members List for a Pool
        //Member Details (for address)
        //Saver Details (for address)
        //Current balance for an address

        //THORName details
        //THORNames reverse lookup
        //THORName owner

        /// <summary>
        /// Gets the <see cref="IAsyncRelayCommand{T}"/> responsible for loading the source markdown docs.
        /// </summary>
        public MainViewModel(DataService dataService, StorageService storageService, MemoryDataCache memoryDataCache) : base()
        {
            this.dataService = dataService;
            this.storageService = storageService;
            this.memoryDataCache = memoryDataCache;
        }

        public Task LoadHealth() => HealthVM.DataLoader.LoadAsync(
            () => memoryDataCache!.GetAsync($"{nameof(LoadHealth)}", 
                () => dataService.MidgardAPI!.GetHealthAsync(), TimeSpan.FromMinutes(1)), 
            x => HealthVM.Data = x);
        public Task LoadChurnList() => ChurnListVM.DataLoader.LoadAsync(
             () => memoryDataCache!.GetAsync($"{nameof(LoadChurnList)}",
                async () => (await dataService.MidgardAPI!.GetChurnsAsync()).ToList(), TimeSpan.FromMinutes(1)),
            x => ChurnListVM.Data = x);
        public Task LoadNodesList() => NodesListVM.DataLoader.LoadAsync(
             () => memoryDataCache!.GetAsync($"{nameof(LoadNodesList)}",
               async () => (await dataService.MidgardAPI!.GetNodesAsync()).ToList(), TimeSpan.FromMinutes(1)),
            x => NodesListVM.Data = x);
        public Task LoadNetworkData() => NetworkDataVM.DataLoader.LoadAsync(
            () => memoryDataCache!.GetAsync($"{nameof(LoadNetworkData)}", 
                () => dataService.MidgardAPI!.GetNetworkDataAsync(), TimeSpan.FromMinutes(1)), 
            x => NetworkDataVM.Data = x);
        public Task LoadStats() => StatsDataVM.DataLoader.LoadAsync(
            () => memoryDataCache!.GetAsync($"{nameof(LoadStats)}", 
                () => dataService.MidgardAPI!.GetStatsAsync(), TimeSpan.FromMinutes(1)), 
            x => StatsDataVM.Data = x);


        public Task LoadKnownPoolsList() => KnownPoolsListVM.DataLoader.LoadAsync(() =>
        {
            return memoryDataCache!.GetAsync($"{nameof(LoadKnownPoolsList)}", async () =>
            {
                var result = await dataService.MidgardAPI!.GetKnownPoolsAsync();
                return result.ToDictionary(x => x.Key, x => x.Value);
            }, TimeSpan.FromMinutes(5));
        }, x => KnownPoolsListVM.Data = x);
        public Task LoadPoolsList() => PoolsListVM.DataLoader.LoadAsync(() =>
        {
            return memoryDataCache!.GetAsync($"{nameof(LoadPoolsList)}", async () =>
            {
                var result = await dataService.MidgardAPI!.GetPoolsAsync(Midgard.Status.Available, null);
                return result.ToList();
            }, TimeSpan.FromMinutes(1));
           
        }, x => PoolsListVM.Data = x);

        public Task LoadPoolDetail(string asset, Period period) => PoolDetailVM.DataLoader.LoadAsync(() =>
        {
            return memoryDataCache!.GetAsync($"{nameof(LoadPoolDetail)}-{asset}-{period}", () =>
            {
                return dataService.MidgardAPI!.GetPoolAsync(asset, period);
            }, TimeSpan.FromMinutes(1));
        }, x => PoolDetailVM.Data = x);

        public Task LoadPoolStatsDetail(string asset, Period2 period) => PoolStatsDetailVM.DataLoader.LoadAsync(() =>
        {
            return memoryDataCache!.GetAsync($"{nameof(LoadPoolDetail)}-{asset}-{period}", () =>
            {
                return dataService.MidgardAPI!.GetPoolStatsAsync(asset, period);
            }, TimeSpan.FromMinutes(1));
        }, x => PoolStatsDetailVM.Data = x);


        public Task LoadPoolDepthHistory(string pool, Interval interval, int count, long? to, long? from)
            => PoolDepthHistoryVM.DataLoader.LoadAsync(() =>
            {
                return memoryDataCache!.GetAsync($"{nameof(LoadPoolDepthHistory)}-{pool}-{interval}-{count}-{to}-{from}", () =>
                {
                    return dataService.MidgardAPI!.GetDepthHistoryAsync(pool, interval, count, to, from);
                }, TimeSpan.FromMinutes(1));
            }, x => PoolDepthHistoryVM.Data = x);

        public Task LoadPoolSwapHistory(string? pool, Interval4 interval, int count, long? to, long? from)
           => PoolSwapHistoryVM.DataLoader.LoadAsync(() =>
           {
               return memoryDataCache!.GetAsync($"{nameof(LoadPoolSwapHistory)}-{pool}-{interval}-{count}-{to}-{from}", () =>
               {
                   return dataService.MidgardAPI!.GetSwapHistoryAsync(pool, interval, count, to, from);
               }, TimeSpan.FromMinutes(1));
           }, x => PoolSwapHistoryVM.Data = x);


        public Task LoadPoolLiquidityHistory(string? pool, Interval3 interval, int count, long? to, long? from)
          => PoolLiquidityHistoryVM.DataLoader.LoadAsync(() =>
          {
              return memoryDataCache!.GetAsync($"{nameof(LoadPoolSwapHistory)}-{pool}-{interval}-{count}-{to}-{from}", () =>
              {
                  return dataService.MidgardAPI!.GetLiquidityHistoryAsync(pool, interval, count, to, from);
              }, TimeSpan.FromMinutes(1));
          }, x => PoolLiquidityHistoryVM.Data = x);



        //TODO: Chart
        public Task LoadEarningshHistory(Interval2 interval, int count, long? to, long? from) => EarningsHistoryVM.DataLoader.LoadAsync(() =>
        {
            return dataService.MidgardAPI!.GetEarningsHistoryAsync(interval, count, to, from);
        }, x => EarningsHistoryVM.Data = x);

        //TODO: Chart
        public Task LoadTotalValueLockedHistory(Interval5 interval, int count, long? to, long? from) => TotalValueLockedHistoryVM.DataLoader.LoadAsync(() =>
        {
            return dataService.MidgardAPI!.GetTVLHistoryAsync(interval, count, to, from);
        }, x => TotalValueLockedHistoryVM.Data = x);



        partial void OnMidgardUrlChanged(string? value)
        {
            memoryDataCache.Clear();

            //Clear all data
            KnownPoolsListVM.Data = null;
            PoolsListVM.Data = null;
            HealthVM.Data = null;
            NetworkDataVM.Data = null;
            StatsDataVM.Data = null;
            ChurnListVM.Data = null;
            NodesListVM.Data = null;
            PoolDetailVM.Data = null;
            PoolStatsDetailVM.Data = null;
            PoolDepthHistoryVM.Data = null;
            EarningsHistoryVM.Data = null;
            PoolSwapHistoryVM.Data = null;
            TotalValueLockedHistoryVM.Data = null;
            PoolLiquidityHistoryVM.Data = null;

            if (!string.IsNullOrEmpty(value))
            {
                storageService.SetApiUrl(value);

                dataService.Init(value);
            }

        }

    }
}
