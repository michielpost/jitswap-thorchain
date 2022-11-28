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

        //protected override async Task OnInitializedAsync()
        //{
        //    await LoadDataAsync();
        //    await base.OnInitializedAsync();
        //}

        protected override async Task OnParametersSetAsync()
        {
            await LoadDataAsync();
            await base.OnParametersSetAsync();
        }

        internal async void BindingContext_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.StateHasChanged();

            if (e.PropertyName == nameof(MainViewModel.MidgardUrl))
            {
                await LoadDataAsync();
                this.StateHasChanged();
            }
        }

        protected virtual Task LoadDataAsync()
        {
            return Task.CompletedTask;
        }
    }
}
