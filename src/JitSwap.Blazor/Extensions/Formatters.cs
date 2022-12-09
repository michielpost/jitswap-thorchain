namespace JitSwap.Blazor.Extensions
{
    public static class Formatters
    {
        public static string ToRune(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            return $"{(Convert.ToInt64(input) / 100000000)} RUNE";
        }
    }
}
