using JitSwap.Blazor.ViewModels;
using static MudBlazor.CategoryTypes;

namespace JitSwap.Blazor.Pages
{
    public partial class Network : MvvmComponentBase<MainViewModel>
    {
        protected override Task LoadDataAsync()
        {
            BindingContext.LoadTotalValueLockedHistory(Midgard.Interval5.Day, 31, null, null);
            BindingContext.LoadChurnList();

            return base.LoadDataAsync();
        }

    }
}
