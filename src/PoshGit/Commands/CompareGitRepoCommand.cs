namespace PoshGit.Commands
{
    using System.Management.Automation;

    using LibGit2Sharp;

    using Microsoft.PowerShell.Commands;

    using PoshGit.Model;

    /// <summary>
    /// The PowerShell equivalent to git diff.
    /// </summary>
    [Cmdlet(VerbsData.Compare, "GitRepo")]
    [OutputType(typeof(Commit))]
    public class CompareGitRepoCommand : PSCmdlet
    {
        /// <summary>
        /// Gets or sets the literal path.
        /// </summary>
        [Parameter]
        public string LiteralPath { get; set; }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(LiteralPath))
            {
                LiteralPath = CurrentProviderLocation(FileSystemProvider.ProviderName).ProviderPath;
            }

            var repoPath = Repository.Discover(LiteralPath);
            using (var repo = new Repository(repoPath))
            {                
            }
        }
    }
}