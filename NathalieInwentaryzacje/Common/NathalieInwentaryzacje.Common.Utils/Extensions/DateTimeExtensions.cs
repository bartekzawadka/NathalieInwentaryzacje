using System;

namespace NathalieInwentaryzacje.Common.Utils.Extensions
{
    public static class DateTimeExtensions
    {
        public static string ToRecordDateString(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public static string ToRecordDateString(this DateTime? dt)
        {
            return dt?.ToString("yyyy-MM-dd");
        }
    }
}
