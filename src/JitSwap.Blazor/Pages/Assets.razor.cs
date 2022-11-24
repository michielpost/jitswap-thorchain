using JitSwap.Blazor.ViewModels;

namespace JitSwap.Blazor.Pages
{
    public partial class Assets : MvvmComponentBase<MainViewModel>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();

            //BindingContext.MidgardUrl = "https://midgard.ninerealms.com";
        }

        
    }
}
