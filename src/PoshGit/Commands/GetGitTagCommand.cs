using System.Linq;
using System.Management.Automation;
using LibGit2Sharp;
using Microsoft.PowerShell.Commands;
using PoshGit.Model;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Get, "GitTag")]
    [OutputType(typeof(TagData))]
    public class GetGitTagCommand : PSCmdlet
    {       

        protected override void ProcessRecord()
        {
            
            var currentDir = CurrentProviderLocation(FileSystemProvider.ProviderName).ProviderPath;

            var repoPath = Repository.Discover(currentDir);
            using (var repo = new Repository(repoPath))
            {                                
                WriteObject(repo.Tags.Select(t=>new TagData(t)), true);                
            }
        }
    }
}