﻿using JitSwap.Blazor.Services;
using JitSwap.Blazor.ViewModels;
using Microsoft.AspNetCore.Components;

namespace JitSwap.Blazor.Shared
{
    public partial class MainLayout
    {
        [Inject]
        public MainViewModel BindingContext { get; set; } = default!;

        [Inject]
        public StorageService StorageService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();

            var apiUrl = await StorageService.GetApiUrl();
            BindingContext.MidgardUrl = apiUrl ?? "https://midgard.ninerealms.com";

        }

    }
}
