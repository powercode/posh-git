namespace PoshGit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using LibGit2Sharp;

    /// <summary>
    /// The git status helper.
    /// </summary>
    internal static class GitStatusHelper
    {
        /// <summary>
        /// The get status enumerator.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="filterPath">
        /// The filter path.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        internal static IEnumerable<StatusEntry> GetStatusEnumerator(Repository repository,  string filterPath)
        {
            Contract.Requires(repository != null);
            Contract.Requires(!string.IsNullOrEmpty(filterPath));

            var workingDirectory = repository.Info.WorkingDirectory.TrimEnd(new[] { '\\' });

            IEnumerable<StatusEntry> status;
            if (string.Compare(workingDirectory, filterPath, StringComparison.OrdinalIgnoreCase) == 0)
            {
                status = repository.Index.RetrieveStatus().OrderBy(GitIndexStatusHelper.Status);
            }
            else
            {                
                var rel = filterPath.Substring(workingDirectory.Length + 1);                
                status =
                    repository.Index.RetrieveStatus()
                        .OrderBy(GitIndexStatusHelper.Status)
                        .Where(fs => fs.FilePath.StartsWith(rel, StringComparison.OrdinalIgnoreCase));
            }

            return status;
        }
    }
}