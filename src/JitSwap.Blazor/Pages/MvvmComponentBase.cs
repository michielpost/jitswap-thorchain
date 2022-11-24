using JitSwap.Blazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace JitSwap.Blazor.Pages
{
    public abstract class MvvmComponentBase<T> : ComponentBase where T : class, System.ComponentModel.INotifyPropertyChanged
    {
        [Inject]
        public T BindingContext { get; set; } = default!;

        protected override void OnInitialized()
        {
            BindingContext.PropertyChanged += BindingContext_PropertyChanged;

            base.OnInitialized();
        }

        internal void BindingContext_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.StateHasChanged();
        }
    }
}
