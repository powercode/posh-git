using System.Linq;
using System.Management.Automation;
using LibGit2Sharp;
using Microsoft.PowerShell.Commands;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Get, "GitBranch")]
    [OutputType(typeof(Branch))]
    public class GetGitBranchCommand : PSCmdlet
    {
        [Parameter]
        public string LiteralPath { get; set; }

        [Parameter]
        public SwitchParameter All { get; set; }

        protected override void ProcessRecord()
        {
            if (string.IsNullOrEmpty(LiteralPath))
            {
                LiteralPath = CurrentProviderLocation(FileSystemProvider.ProviderName).ProviderPath;
            }
            var repoPath = Repository.Discover(LiteralPath);
            using (var repo = new Repository(repoPath))
            {
                
                if (All)
                {
                    WriteObject(repo.Branches, true);
                }
                else
                {
                    WriteObject(repo.Branches.Where(b=>b.IsRemote == false), true);
                }
            }
        }
    }
}