using System.Globalization;

namespace PoshGit.Model
{
    internal class ResourceStrings
    {
        public static string Format(string format, object obj)
        {
            return string.Format(CultureInfo.CurrentCulture, format, obj);
        }

        public static string Format(string format, object obj, object obj1)
        {
            return string.Format(CultureInfo.CurrentCulture, format, obj, obj1);
        }
    }
}