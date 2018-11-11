using System;

namespace TelFlix.App.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string TruncateAtWord(this String input, int length)
        {
            if (input == null || input.Length < length)
            {
                return input;
            }

            int nextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);
            return string.Format("{0}...", input.Substring(0, (nextSpace > 0) ? nextSpace : length).Trim());
        }
    }
}
