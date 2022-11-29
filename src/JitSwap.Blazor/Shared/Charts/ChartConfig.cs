using ApexCharts;
using Microsoft.Extensions.Options;
using Midgard;

namespace JitSwap.Blazor.Shared.Charts
{
    public static class ChartConfig
    {
        public static ApexChartOptions<T> GetDefaultChartOptions<T>() where T : class
        {
            return new ApexChartOptions<T>
            {
                Theme = new Theme
                {
                    Mode = Mode.Dark,
                    Palette = PaletteType.Palette1
                }
            };

        }
      
    }
}
