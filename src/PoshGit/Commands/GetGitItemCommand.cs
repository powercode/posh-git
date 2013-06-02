namespace PoshGit.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Management.Automation;

    using LibGit2Sharp;

    using PoshGit.Model;

    /// <summary>
    /// The get git item command.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "GitItem")]
    [OutputType(typeof(GitItemData))]
    public sealed class GetGitItemCommand : PSCmdlet, IDisposable
    {
        /// <summary>
        /// Gets or sets the literal path.
        /// </summary>
        [Alias("PSPath", "RepositoryPath")]
        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true)]
        public string LiteralPath { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true)]
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets the include.
        /// </summary>
        [Parameter(Position = 1)]
        public string[] Include { get; set; }

        /// <summary>
        /// Gets or sets the exclude.
        /// </summary>
        [Parameter]
        public string[] Exclude { get; set; }

        /// <summary>
        /// The repository.
        /// </summary>
        private Repository repository;

        /// <summary>
        /// The repository path.
        /// </summary>
        private string repositoryPath;

        /// <summary>
        /// The include patterns.
        /// </summary>
        private WildcardPattern[] includePatterns;

        /// <summary>
        /// The exclude patterns.
        /// </summary>
        private WildcardPattern[] excludePatterns;

        /// <summary>
        /// The should write item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool ShouldWriteItem(GitItemData item)
        {
            if (excludePatterns != null && excludePatterns.Any(p => p.IsMatch(item.Fullname)))
            {
                return false;
            }

            if (includePatterns != null && !includePatterns.Any(p => p.IsMatch(item.Fullname)))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The begin processing.
        /// </summary>
        protected override void BeginProcessing()
        {
            if (Include != null)
            {
                includePatterns = CreatePatterns(Include);
            }

            if (Exclude != null)
            {
                excludePatterns = CreatePatterns(Exclude);
            }

            if (string.IsNullOrEmpty(LiteralPath))
            {
                LiteralPath = SessionState.Path.CurrentFileSystemLocation.ProviderPath;
            }

            repositoryPath = Repository.Discover(LiteralPath);
            if (repositoryPath == null)
            {
                ExceptionHelper.ThrowInvalidRepositoryPath(LiteralPath);
            }

            repository = new Repository(repositoryPath);
        }

        /// <summary>
        /// The create patterns.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <returns>
        /// The <see cref="WildcardPattern[]"/>.
        /// </returns>
        private static WildcardPattern[] CreatePatterns(IEnumerable<string> pattern)
        {
            Contract.Requires(pattern != null);
            return (from p in pattern select new WildcardPattern(p, WildcardOptions.IgnoreCase)).ToArray();
        }

        /// <summary>
        /// The end processing.
        /// </summary>
        protected override void EndProcessing()
        {
            if (repository != null)
            {
                repository.Dispose();
            }
        }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                var commit = repository.Lookup<Commit>(Id);
                var parent = commit.Parents.FirstOrDefault();
                {
                    var parentTree = parent == null ? null : parent.Tree;
                    var changes = repository.Diff.Compare(parentTree, commit.Tree);
                    WriteObject(changes, true);
                }
            }
            catch
            {
                repository.Dispose();
                repository = null;
            }
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            if (repository != null)
            {
                repository.Dispose();
            }
        }
    }
}