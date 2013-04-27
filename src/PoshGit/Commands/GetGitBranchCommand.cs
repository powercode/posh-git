using System.Linq;
using System.Management.Automation;
using LibGit2Sharp;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Get, "GitBranch")]
    [OutputType(typeof (Branch))]
    public class GetGitBranchCommand : PSCmdlet
    {
        [Alias("PSPath")]
        [Parameter(ValueFromPipelineByPropertyName = true)]
        public string LiteralPath { get; set; }

        [Parameter]
        public SwitchParameter All { get; set; }

        protected override void ProcessRecord()
        {
            LiteralPath = string.IsNullOrEmpty(LiteralPath)
                              ? SessionState.Path.CurrentFileSystemLocation.ProviderPath
                              : GetUnresolvedProviderPathFromPSPath(LiteralPath);
            var repoPath = Repository.Discover(LiteralPath);
            using (var repo = new Repository(repoPath))
            {
                WriteObject(All ? repo.Branches : repo.Branches.Where(b => b.IsRemote == false), true);
            }
        }
    }
}