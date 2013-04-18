using System.IO;
using System.Management.Automation;
using LibGit2Sharp;
using Microsoft.PowerShell.Commands;
using PoshGit.Model;
using PoshGit.Properties;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.New, "GitRepository")]
    [OutputType(typeof (DirectoryInfo))]
    public class NewGitRepositoryCommand : PSCmdlet
    {
        [Parameter(Position = 1)]
        public string LiteralPath { get; set; }

        [Parameter]
        public SwitchParameter Bare { get; set; }

        protected override void ProcessRecord()
        {
            LiteralPath = string.IsNullOrEmpty(LiteralPath)
                              ? CurrentProviderLocation(FileSystemProvider.ProviderName).ProviderPath
                              : GetUnresolvedProviderPathFromPSPath(LiteralPath);
            if (!Directory.Exists(LiteralPath))
            {
                Directory.CreateDirectory(LiteralPath);                
            }
            using (Repository.Init(LiteralPath, Bare.IsPresent))
            {
            }
            WriteObject(new DirectoryInfo(LiteralPath));
        }
    }
}