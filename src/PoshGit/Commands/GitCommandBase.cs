namespace PoshGit.Commands
{    
    using System.Diagnostics.Contracts;
    using System.Management.Automation;

    using LibGit2Sharp;

    using PoshGit.Model;

    /// <summary>
    /// The git command base.
    /// </summary>
    public class GitCommandBase : PSCmdlet
    {
        /// <summary>
        /// The literal path.
        /// </summary>
        private string literalPath;

        /// <summary>
        ///     Gets or sets the literal path to the item of which to get commits.
        /// </summary>
        [Alias("PSPath")]
        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string LiteralPath
        {
            get
            {
                return string.IsNullOrEmpty(literalPath) ? 
                        SessionState.Path.CurrentFileSystemLocation.ProviderPath 
                        : literalPath;
            }

            set
            {
                literalPath = value;
            }
        }

        /// <summary>
        /// Gets a libgit2Sharp repository for the fullPath.
        /// </summary>
        /// <param name="fullPath">
        /// The full path.
        /// </param>
        /// <returns>
        /// The <see cref="Repository"/>.
        /// </returns>
        protected Repository GetRepository(string fullPath)
        {
            Contract.Requires(!string.IsNullOrEmpty(fullPath));

            return GitRepositoryFactory.Instance.GetRepository(fullPath);
        }

        /// <summary>
        /// The get repository at LiteralPath.
        /// </summary>
        /// <returns>
        /// The <see cref="Repository"/>.
        /// </returns>
        protected Repository GetLiteralPathRepository()
        {
            return GitRepositoryFactory.Instance.GetRepository(LiteralPath);
        } 
    }
}