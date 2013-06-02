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
        /// <param name="fullname">
        /// The full name.
        /// </param>
        /// <param name="filterPath">
        /// The filter path.
        /// </param>
        /// <returns>
        /// The <see cref="StatusEnumerator"/>.
        /// </returns>
        internal static StatusEnumerator GetStatusEnumerator(Repository repository, string fullname, string filterPath)
        {
            Contract.Requires(repository != null);
            Contract.Requires(!string.IsNullOrEmpty(fullname));
            try
            {
                IEnumerable<StatusEntry> status;
                if (fullname == filterPath || string.IsNullOrEmpty(filterPath))
                {
                    status = repository.Index.RetrieveStatus().OrderBy(GitIndexStatusHelper.Status);
                }
                else
                {
                    var rel = filterPath.Substring(repository.Info.Path.Length - 5);
                    status =
                        repository.Index.RetrieveStatus()
                            .OrderBy(GitIndexStatusHelper.Status)
                            .Where(fs => fs.FilePath.StartsWith(rel, StringComparison.OrdinalIgnoreCase));
                }

                return new StatusEnumerator(repository, status);
            }
            catch
            {
                repository.Dispose();
                throw;
            }
        }
    }

    /// <summary>
    ///     The status enumerator.
    /// </summary>
    internal class StatusEnumerator : IDisposable
    {
        /// <summary>
        ///     The repo.
        /// </summary>
        private readonly Repository repo;

        /// <summary>
        /// The status.
        /// </summary>
        private readonly IEnumerable<StatusEntry> status;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusEnumerator"/> class.
        /// </summary>
        /// <param name="repo">
        /// The repo.
        /// </param>
        /// <param name="status">
        /// The status.
        /// </param>
        public StatusEnumerator(Repository repo, IEnumerable<StatusEntry> status)
        {
            this.repo = repo;
            this.status = status;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="StatusEnumerator"/> class. 
        /// </summary>
        ~StatusEnumerator()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        public IEnumerable<StatusEntry> Status
        {
            get
            {
                return status;
            }
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposed">
        /// The disposed.
        /// </param>
        private void Dispose(bool disposed)
        {
            if (disposed)
            {
                repo.Dispose();
            }
        }
    }
}