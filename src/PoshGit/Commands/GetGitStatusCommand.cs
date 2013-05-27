using System;
using System.Management.Automation;
using LibGit2Sharp;
using PoshGit.Model;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Get, "GitStatus")]
    [OutputType(typeof (StatusEntry))]
    public class GetGitStatusCommand : PSCmdlet
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
        [Alias("PSPath")]
        public string[] LiteralPath { get; set; }


        protected override void ProcessRecord()
        {

            if (LiteralPath == null)
            {
                LiteralPath = new[] {SessionState.Path.CurrentFileSystemLocation.ProviderPath};
            }
            foreach(var p in LiteralPath){
                var statusPath = GetUnresolvedProviderPathFromPSPath(p);                                        

                var filterpath = statusPath;
                
                using(var statusEnumerator = GitStatusHelper.GetStatusEnumerator(statusPath, filterpath)){
                    WriteObject(statusEnumerator.Status, true);
                }
            }
        }
    }
}