using System.Linq;
using System.Management.Automation;
using LibGit2Sharp;
using PoshGit.Model;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Get, "GitCommit", SupportsPaging = true)]
    [OutputType(typeof (CommitData))]
    public class GetGitCommitCommand : PSCmdlet
    {        
        [Alias("Branch","Target")]
        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true)]
        public string Reference { get; set; }
       
        [Parameter]
        public string Until { get; set; }

        [Alias("PSPath")]
        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true)]
        public string LiteralPath { get; set; }

        [Parameter()]
        public SwitchParameter TopologicalOrder { get; set; }

        [Parameter()]
        public SwitchParameter ReverseOrder { get; set; }

        public GetGitCommitCommand()
        {
            Reference = "HEAD";
        }

        
        protected override void ProcessRecord()
        {
            LiteralPath = string.IsNullOrEmpty(LiteralPath)
                              ? SessionState.Path.CurrentFileSystemLocation.ProviderPath
                              : GetUnresolvedProviderPathFromPSPath(LiteralPath);

            var filter = new Filter(){Since = Reference};
            if (TopologicalOrder.IsPresent)
            {
                filter.SortBy |= GitSortOptions.Topological;
            }
            if (ReverseOrder.IsPresent)
            {
                filter.SortBy |= GitSortOptions.Reverse;
            }

            if (MyInvocation.BoundParameters.ContainsKey("Until"))
            {
                filter.Until = Until;
            }

            using (var commitEnumerator = GitCommitHelper.EnumerateCommits(LiteralPath, filter))
            {
                var commits = commitEnumerator.Commits;
                if (PagingParameters.IncludeTotalCount)
                {
                    WriteObject(PagingParameters.NewTotalCount(100, 0.0));
                }
                
                if (PagingParameters.Skip != 0)
                {
                     commits = commits.Skip((int)PagingParameters.Skip);
                }
                if (PagingParameters.First != ulong.MaxValue)
                {
                    commits = commits.Take((int)PagingParameters.First);
                }

                WriteObject(commits, true);
            }
        }
    }
}