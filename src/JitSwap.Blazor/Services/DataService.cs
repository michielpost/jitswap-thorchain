using JitSwap.Blazor.ViewModels;
using Midgard;

namespace JitSwap.Blazor.Services
{
    public class DataService
    {
        public MidgardAPI? MidgardAPI { get; set; }

        internal void Init(string url)
        {
            var httpClient = new HttpClient();
            MidgardAPI = new Midgard.MidgardAPI(url, httpClient);
        }
    }
}
