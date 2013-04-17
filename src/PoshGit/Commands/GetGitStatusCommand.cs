using System;
using System.Management.Automation;
using LibGit2Sharp;
using PoshGit.Model;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Get, "GitStatus")]
    [OutputType(typeof (RepositoryStatus))]
    public class GetGitStatusCommand : PSCmdlet
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
        [Alias("PSPath")]
        public string LiteralPath { get; set; }


        protected override void ProcessRecord()
        {
            var statusPath = String.IsNullOrEmpty(LiteralPath)
                                    ? SessionState.Path.CurrentFileSystemLocation.ProviderPath
                                    : GetUnresolvedProviderPathFromPSPath(LiteralPath);

            var filterpath = statusPath;
            if (!String.IsNullOrEmpty(LiteralPath))
            {
                filterpath = GetUnresolvedProviderPathFromPSPath(LiteralPath);
            }

            using(var statusEnumerator = GitStatusHelper.GetStatusEnumerator(statusPath, filterpath)){
                WriteObject(statusEnumerator.Status, true);
            }
        }
    }
}