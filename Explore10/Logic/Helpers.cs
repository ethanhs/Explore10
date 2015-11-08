using System;

namespace Explore10
{
    public static class Helpers
    {
        static readonly string[] Suffixes =
        { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string PrettyByte(long value)
        {
            if (value == 0) { return "0.0 bytes"; }

            var mag = (int)Math.Log(value, 1024);
            var adjustedSize = (decimal)value / (1L << (mag * 10));

            return $"{adjustedSize:n1} {Suffixes[mag]}";
        }
    }
}
