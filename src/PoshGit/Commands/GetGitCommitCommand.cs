using System.Management.Automation;
using PoshGit.Model;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Get, "GitCommit")]
    [OutputType(typeof (CommitData))]
    public class GetGitCommitCommand : PSCmdlet
    {
        [Alias("Branch")]
        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true, ValueFromPipeline = true)]
        public string Reference { get; set; }

        [Alias("PSPath")]
        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true)]
        public string LiteralPath { get; set; }


        protected override void ProcessRecord()
        {
            LiteralPath = string.IsNullOrEmpty(LiteralPath)
                              ? SessionState.Path.CurrentFileSystemLocation.ProviderPath
                              : GetUnresolvedProviderPathFromPSPath(LiteralPath);

            using (var commitEnumerator = GitLogHelper.EnumerateCommits(LiteralPath, Reference))
            {
                WriteObject(commitEnumerator.Commits, true);
            }
        }
    }
}