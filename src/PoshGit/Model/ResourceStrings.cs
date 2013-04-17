using System.Globalization;

namespace PoshGit.Model
{
    public class ResourceStrings
    {
        public static string Format(string format, object obj)
        {
            return string.Format(CultureInfo.CurrentCulture, format, obj);
        }
    }
}