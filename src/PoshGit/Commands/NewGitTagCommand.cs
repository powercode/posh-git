using System.Management.Automation;
using LibGit2Sharp;
using Microsoft.PowerShell.Commands;
using Signature = LibGit2Sharp.Signature;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.New, "GitTag", SupportsPaging = true)]
    [OutputType(typeof (Tag))]
    public class NewGitTagCommand : PSCmdlet
    {
        [Parameter]
        public string LiteralPath { get; set; }

        [Parameter(Mandatory = true)]
        public string Name { get; set; }

        [Parameter(Mandatory = true)]
        public string Target { get; set; }

        [Parameter(Mandatory = true)]
        public Signature Signature { get; set; }


        [Parameter]
        public string Message { get; set; }

        protected override void ProcessRecord()
        {
            LiteralPath = string.IsNullOrEmpty(LiteralPath)
                                          ? SessionState.Path.CurrentFileSystemLocation.ProviderPath
                                          : GetUnresolvedProviderPathFromPSPath(LiteralPath);

            var repoPath = Repository.Discover(LiteralPath);

            using (var repo = new Repository(repoPath))
            {
                repo.Tags.Add(Name, Target, Signature, Message);
            }
        }

    }
}