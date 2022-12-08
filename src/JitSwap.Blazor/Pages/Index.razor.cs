using JitSwap.Blazor.ViewModels;

namespace JitSwap.Blazor.Pages
{
    public partial class Index : MvvmComponentBase<MainViewModel>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async Task LoadDataAsync()
        {
            BindingContext.LoadHealth();
            BindingContext.LoadNetworkData();
            BindingContext.LoadStats();
            BindingContext.LoadChurnList();
        }

    }
}
