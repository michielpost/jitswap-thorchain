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
        [NotifyPropertyChangedRecipients]
        private string? selectedAsset;

        //[ObservableProperty]
        //[NotifyPropertyChangedRecipients]
        //private string? currentAsset;

        [ObservableProperty]
        private DataLoader knownPoolsListDataLoader = new();

        [ObservableProperty]
        private Dictionary<string, string>? knownPoolsList;

        [ObservableProperty]
        private DataLoader poolsListDataLoader = new();

        [ObservableProperty]
        private List<PoolDetail>? poolsList;

        [ObservableProperty]
        private DataLoader healthDataLoader = new();

        [ObservableProperty]
        private Health? health;

        [ObservableProperty]    
        private DataLoader networkDataDataLoader = new();

        [ObservableProperty]
        private Network? networkData;

        [ObservableProperty]
        private DataLoader statsDataDataLoader = new();

        [ObservableProperty]
        private StatsData? statsData;

        [ObservableProperty]
        private DataLoader churnListDataLoader = new();

        [ObservableProperty]
        private List<ChurnItem>? churnList;

        [ObservableProperty]
        private DataLoader nodesListDataLoader = new();

        [ObservableProperty]
        private List<Node>? nodesList;

        [ObservableProperty]
        private DataLoader poolDetailDataLoader = new();

        [ObservableProperty]
        private PoolDetail? poolDetail;

        [ObservableProperty]
        private DataLoader poolStatsDetailDataLoader = new();

        [ObservableProperty]
        private PoolStatsDetail? poolStatsDetail;

        [ObservableProperty]
        private DataLoader poolDepthHistoryDataLoader = new();

        [ObservableProperty]
        private DepthHistory? poolDepthHistory;

        [ObservableProperty]
        private DataLoader earningsHistoryDataLoader = new();

        [ObservableProperty]
        private EarningsHistory? earningsHistory;

        [ObservableProperty]
        private DataLoader poolSwapHistoryDataLoader = new();

        [ObservableProperty]
        private SwapHistory? poolSwapHistory;

        [ObservableProperty]
        private DataLoader totalValueLockedHistoryDataLoader = new();

        [ObservableProperty]
        private TVLHistory? totalValueLockedHistory;

        [ObservableProperty]
        private DataLoader poolLiquidityHistoryDataLoader = new();

        [ObservableProperty]
        private LiquidityHistory? poolLiquidityHistory;

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

        public Task LoadHealth() => healthDataLoader.LoadAsync(() => dataService.MidgardAPI!.GetHealthAsync(), x => Health = x);
        public Task LoadChurnList() => churnListDataLoader.LoadAsync(async () => (await dataService.MidgardAPI!.GetChurnsAsync()).ToList(), x => ChurnList = x);
        public Task LoadNodesList() => nodesListDataLoader.LoadAsync(async () => (await dataService.MidgardAPI!.GetNodesAsync()).ToList(), x => NodesList = x);
        public Task LoadNetworkData() => networkDataDataLoader.LoadAsync(() => dataService.MidgardAPI!.GetNetworkDataAsync(), x => NetworkData = x);
        public Task LoadStats() => statsDataDataLoader.LoadAsync(() => dataService.MidgardAPI!.GetStatsAsync(), x => StatsData = x);


        public Task LoadKnownPoolsList() => knownPoolsListDataLoader.LoadAsync(async () =>
        {
            var result = await dataService.MidgardAPI!.GetKnownPoolsAsync();
            return result.ToDictionary(x => x.Key, x => x.Value);
        }, x => KnownPoolsList = x);
        public Task LoadPoolsList() => poolsListDataLoader.LoadAsync(async () =>
        {
            var result = await dataService.MidgardAPI!.GetPoolsAsync(Midgard.Status.Available, null);
            return result.ToList();
        }, x => PoolsList = x);

        public Task LoadPoolDetail(string asset, Period period) => poolDetailDataLoader.LoadAsync(() =>
        {
            return dataService.MidgardAPI!.GetPoolAsync(asset, period);
        }, x => PoolDetail = x);

        public Task LoadPoolStatsDetail(string asset, Period2 period) => poolStatsDetailDataLoader.LoadAsync(() =>
        {
            return dataService.MidgardAPI!.GetPoolStatsAsync(asset, period);
        }, x => PoolStatsDetail = x);


        public Task LoadPoolDepthHistory(string pool, Interval interval, int count, long? to, long? from)
            => poolDepthHistoryDataLoader.LoadAsync(() =>
            {
                return memoryDataCache!.GetAsync($"{nameof(LoadPoolDepthHistory)}-{pool}-{interval}-{count}-{to}-{from}", () =>
                {
                    return dataService.MidgardAPI!.GetDepthHistoryAsync(pool, interval, count, to, from);
                }, DateTimeOffset.UtcNow.AddMinutes(1));
            }, x => PoolDepthHistory = x);

        public Task LoadPoolSwapHistory(string? pool, Interval4 interval, int count, long? to, long? from)
           => poolSwapHistoryDataLoader.LoadAsync(() =>
           {
               return memoryDataCache!.GetAsync($"{nameof(LoadPoolSwapHistory)}-{pool}-{interval}-{count}-{to}-{from}", () =>
               {
                   return dataService.MidgardAPI!.GetSwapHistoryAsync(pool, interval, count, to, from);
               }, DateTimeOffset.UtcNow.AddMinutes(1));
           }, x => PoolSwapHistory = x);


        //TODO: Chart
        public Task LoadPoolLiquidityHistory(string? pool, Interval3 interval, int count, long? to, long? from)
          => poolLiquidityHistoryDataLoader.LoadAsync(() =>
          {
              return dataService.MidgardAPI!.GetLiquidityHistoryAsync(pool, interval, count, to, from);
          }, x => PoolLiquidityHistory = x);



        //TODO: Chart
        public Task LoadEarningshHistory(Interval2 interval, int count, long? to, long? from) => earningsHistoryDataLoader.LoadAsync(() =>
        {
            return dataService.MidgardAPI!.GetEarningsHistoryAsync(interval, count, to, from);
        }, x => EarningsHistory = x);

        //TODO: Chart
        public Task LoadTotalValueLockedHistory(Interval5 interval, int count, long? to, long? from) => totalValueLockedHistoryDataLoader.LoadAsync(() =>
        {
            return dataService.MidgardAPI!.GetTVLHistoryAsync(interval, count, to, from);
        }, x => TotalValueLockedHistory = x);



        partial void OnMidgardUrlChanged(string? value)
        {
            memoryDataCache.Clear();

            if (!string.IsNullOrEmpty(value))
            {
                storageService.SetApiUrl(value);

                dataService.Init(value);
            }

        }

    }
}
