namespace PoshGit.Commands
{
    using System.Linq;
    using System.Management.Automation;

    using LibGit2Sharp;

    using Microsoft.PowerShell.Commands;

    using PoshGit.Model;

    /// <summary>
    /// The PowerShell equivalent to the "git tag" command.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "GitTag")]
    [OutputType(typeof(TagData))]
    public class GetGitTagCommand : PSCmdlet
    {
        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {            
            var currentDir = CurrentProviderLocation(FileSystemProvider.ProviderName).ProviderPath;

            var repoPath = Repository.Discover(currentDir);
            using (var repo = new Repository(repoPath))
            {
                WriteObject(repo.Tags.Select(t => new TagData(t)), true);
            }
        }
    }
}