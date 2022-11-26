using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using JitSwap.Blazor.Services;
using Midgard;

namespace JitSwap.Blazor.ViewModels
{
    public partial class MainViewModel : ObservableRecipient
    {
        private readonly DataService dataService;

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private string? midgardUrl;

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<List<string>> knownPoolsList = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<List<PoolDetail>> poolsList = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<Health> health = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<Network> networkData = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<StatsData> statsData = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<List<ChurnItem>> churnList = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<List<Node>> nodesList = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<PoolDetail> poolDetail = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients] 
        private DataLoader<PoolStatsDetail> poolStatsDetail = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<DepthHistory> poolDepthHistory = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<EarningsHistory> earningsHistory = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<SwapHistory> poolSwapHistory = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<TVLHistory> totalValueLockedHistory = new();

        [ObservableProperty]
        [NotifyPropertyChangedRecipients]
        private DataLoader<LiquidityHistory> poolLiquidityHistory = new();

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
        public MainViewModel(DataService dataService) : base()
        { 
            this.dataService = dataService;
        }

        public Task LoadHealth() => health.LoadAsync(() => dataService.MidgardAPI!.GetHealthAsync());
        public Task LoadChurnList() => churnList.LoadAsync(async () => (await dataService.MidgardAPI!.GetChurnsAsync()).ToList());
        public Task LoadNodesList() => nodesList.LoadAsync(async () => (await dataService.MidgardAPI!.GetNodesAsync()).ToList());
        public Task LoadNetworkData() => networkData.LoadAsync(() => dataService.MidgardAPI!.GetNetworkDataAsync());
        public Task LoadStats() => statsData.LoadAsync(() => dataService.MidgardAPI!.GetStatsAsync());


        public Task LoadKnownPoolsList() => knownPoolsList.LoadAsync(async () => {
            var apiAssets = await dataService.MidgardAPI!.GetKnownPoolsAsync();
            return apiAssets.Where(x => x.Value == "available").Select(x => x.Key).ToList();
        });
        public Task LoadPoolsList() => poolsList.LoadAsync(async () => {
            var apiAssets = await dataService.MidgardAPI!.GetPoolsAsync(Midgard.Status.Available, null);
            return apiAssets.Where(x => x.Status == "available").Select(x => x).ToList();
        });

        public Task LoadPoolDetail(string asset, Period period) => poolDetail.LoadAsync(() => {
            return dataService.MidgardAPI!.GetPoolAsync(asset, period);
        });

        public Task LoadPoolStatsDetail(string asset, Period2 period) => poolStatsDetail.LoadAsync(() => {
            return dataService.MidgardAPI!.GetPoolStatsAsync(asset, period);
        });

        public Task LoadPoolDepthHistory(string pool, Interval interval, int count, long? to, long? from) => poolDepthHistory.LoadAsync(() => {
            return dataService.MidgardAPI!.GetDepthHistoryAsync(pool, interval, count, to, from);
        });

        public Task LoadPoolSwapHistory(string? pool, Interval4 interval, int count, long? to, long? from)
           => poolSwapHistory.LoadAsync(() => {
               return dataService.MidgardAPI!.GetSwapHistoryAsync(pool, interval, count, to, from);
           });

        public Task LoadPoolLiquidityHistory(string? pool, Interval3 interval, int count, long? to, long? from)
          => poolLiquidityHistory.LoadAsync(() => {
              return dataService.MidgardAPI!.GetLiquidityHistoryAsync(pool, interval, count, to, from);
          });




        public Task LoadEarningshHistory(Interval2 interval, int count, long? to, long? from) => earningsHistory.LoadAsync(() => {
            return dataService.MidgardAPI!.GetEarningsHistoryAsync(interval, count, to, from);
        });

        public Task LoadTotalValueLockedHistory(Interval5 interval, int count, long? to, long? from) => totalValueLockedHistory.LoadAsync(() => {
            return dataService.MidgardAPI!.GetTVLHistoryAsync(interval, count, to, from);
        });



        partial void OnMidgardUrlChanged(string? value)
        {
            if(!string.IsNullOrEmpty(value))
            {
                dataService.Init(value);

                knownPoolsList.Clear();
                poolsList.Clear();
                health.Clear();
                networkData.Clear();
                statsData.Clear();
                churnList.Clear();
                nodesList.Clear();
                poolDetail.Clear();
                poolStatsDetail.Clear();
                poolDepthHistory.Clear();
                earningsHistory.Clear();
                poolSwapHistory.Clear();
                totalValueLockedHistory.Clear();
                poolLiquidityHistory.Clear();

                this.InitializeAsync();
            }
           
        }

        private async Task InitializeAsync()
        {
            //await LoadKnownPoolsAsync();
            //await LoadPoolDetailsAsync();
        }

       
        
    }
}
