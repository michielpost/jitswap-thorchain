namespace JitSwap.Blazor.Extensions
{
    public static class DateTimeOffsetExtensions
    {
        public static DateTimeOffset FromUnixTimeNanoseconds(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            return dateTime.AddSeconds(unixTimeStamp / 1000000000);
        }
    }
}
