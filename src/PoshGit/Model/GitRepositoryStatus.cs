namespace PoshGit.Model
{
    using System.Diagnostics.Contracts;
    using System.Linq;

    using LibGit2Sharp;

    /// <summary>
    /// The git repository status.
    /// </summary>
    public class GitRepositoryStatus
    {               
        /// <summary>
        /// Initializes a new instance of the <see cref="GitRepositoryStatus"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public GitRepositoryStatus(IRepository repository)
        {
            Contract.Requires(repository != null);
            var status = repository.Index.RetrieveStatus();
            UntrackedCount = status.Untracked.Count();
        }

        /// <summary>
        /// Gets or sets the untracked count.
        /// </summary>
        public int UntrackedCount { get; set; }
    }
}
