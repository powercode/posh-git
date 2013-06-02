namespace PoshGit.Model
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Helper class to make use of resource strings easier.
    /// </summary>
    internal class ResourceStrings
    {
        /// <summary>
        /// Formats a string with the current culture.
        /// </summary>
        /// <param name="format">
        /// The format string
        /// </param>
        /// <param name="obj">
        /// The object to use in the format string
        /// </param>
        /// <returns>
        /// The formatted <see cref="string"/>.
        /// </returns>
        public static string Format(string format, object obj)
        {
            Contract.Requires(!string.IsNullOrEmpty(format));            
            return string.Format(CultureInfo.CurrentCulture, format, obj);
        }

        /// <summary>
        /// Formats a string with the current culture
        /// </summary>
        /// <param name="format">
        /// The format string
        /// </param>
        /// <param name="obj">
        /// The object to format.
        /// </param>
        /// <param name="obj1">
        /// The second object to format.
        /// </param>
        /// <returns>
        /// The formatted <see cref="string"/>.
        /// </returns>
        public static string Format(string format, object obj, object obj1)
        {
            Contract.Requires(!String.IsNullOrEmpty(format));
            return string.Format(CultureInfo.CurrentCulture, format, obj, obj1);
        }
    }
}