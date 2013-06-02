using System.Collections.Generic;

using LibGit2Sharp;

namespace PoshGit.Model
{
    using System.Diagnostics.Contracts;

    internal static class GitCommitHelper
    {
        /// <summary>
        /// The enumerate commits.
        /// </summary>
        /// <param name="repo">
        /// The repo.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        internal static IEnumerable<Commit> EnumerateCommits(Repository repo, Filter filter)
        {
            Contract.Requires(repo != null);
            Contract.Requires(filter != null);                                                
            var commits = repo.Commits.QueryBy(filter);
            return commits;
        }        
    }
}