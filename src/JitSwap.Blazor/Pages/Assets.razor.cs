using JitSwap.Blazor.ViewModels;
using static MudBlazor.CategoryTypes;

namespace JitSwap.Blazor.Pages
{
    public partial class Assets : MvvmComponentBase<MainViewModel>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async Task LoadDataAsync()
        {
            BindingContext.LoadPoolsList();
        }

    }
}
