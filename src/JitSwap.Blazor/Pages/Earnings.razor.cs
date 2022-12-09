using JitSwap.Blazor.ViewModels;
using static MudBlazor.CategoryTypes;

namespace JitSwap.Blazor.Pages
{
    public partial class Earnings : MvvmComponentBase<MainViewModel>
    {
        protected override Task LoadDataAsync()
        {
            BindingContext.LoadEarningshHistory(Midgard.Interval2.Day, 31, null, null);

            return base.LoadDataAsync();
        }

    }
}
