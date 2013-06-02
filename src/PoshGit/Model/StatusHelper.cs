namespace PoshGit.Model
{
    using System.Diagnostics.Contracts;

    using LibGit2Sharp;

    /// <summary>
    /// The status helper.
    /// </summary>
    public class StatusHelper
    {
        /// <summary>
        /// The _repository status.
        /// </summary>
        private readonly RepositoryStatus repositoryStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusHelper"/> class.
        /// </summary>
        /// <param name="repositoryStatus">
        /// The repository status.
        /// </param>
        private StatusHelper(RepositoryStatus repositoryStatus)
        {
            Contract.Requires(repositoryStatus != null);
            this.repositoryStatus = repositoryStatus;
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        public RepositoryStatus Status
        {
            get
            {
                return repositoryStatus;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is dirty.
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return repositoryStatus.IsDirty;
            }
        }

        /// <summary>
        /// The get status.
        /// </summary>
        /// <param name="directory">
        /// The directory.
        /// </param>
        /// <returns>
        /// The <see cref="StatusHelper"/>.
        /// </returns>
        public static StatusHelper GetStatus(string directory)
        {
            using (var repo = new Repository(Repository.Discover(directory)))
            {
                return new StatusHelper(repo.Index.RetrieveStatus());
            }
        }
    }
}