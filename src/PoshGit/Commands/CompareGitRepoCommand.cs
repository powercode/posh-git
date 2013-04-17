using System.Management.Automation;
using LibGit2Sharp;
using Microsoft.PowerShell.Commands;
using PoshGit.Model;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsData.Compare, "GitRepo")]
    [OutputType(typeof (CommitData))]
    public class CompareGitRepoCommand : PSCmdlet
    {
        [Parameter]
        public string LiteralPath { get; set; }

        protected override void ProcessRecord()
        {

            if (string.IsNullOrEmpty(LiteralPath))
            {
                LiteralPath = CurrentProviderLocation(FileSystemProvider.ProviderName).ProviderPath;
            }
            var repoPath = Repository.Discover(LiteralPath);
            using (var repo = new Repository(repoPath))
            {
//repo.Diff.Compare()
            }
        }
    }
}