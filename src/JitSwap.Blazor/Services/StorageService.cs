using Blazored.LocalStorage;

namespace JitSwap.Blazor.Services
{
    public class StorageService
    {
        private readonly ILocalStorageService localStorage;

        private const string API_URL_KEY = "API_URL";
        public StorageService(ILocalStorageService localStorage)
        {
            this.localStorage = localStorage;
        }

        public ValueTask<string> GetApiUrl()
        {
            return localStorage.GetItemAsync<string>(API_URL_KEY);
        }

        public ValueTask SetApiUrl(string apiUrl)
        {
            return localStorage.SetItemAsync(API_URL_KEY, apiUrl);
        }

    }
}
