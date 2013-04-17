using System.Management.Automation;
using LibGit2Sharp;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Get, "GitContent")]
    [OutputType(typeof (string))]
    [OutputType(typeof (byte[]))]
    public class GetGitContentCommand : PSCmdlet
    {
        [Alias("PSPath", "RepositoryPath")]
        [Parameter(Mandatory = true, Position = 2, ValueFromPipelineByPropertyName = true)]
        public string LiteralPath { get; set; }

        [Parameter(Position = 1, ValueFromPipelineByPropertyName = true)]
        public ObjectId Id { get; set; }

        [Parameter]
        public SwitchParameter ExcludeBinary { get; set; }
        
        private string repoPath;
        private Repository repository;

        protected override void ProcessRecord()
        {
            if (repoPath != LiteralPath)
            {
                if (repository != null)
                {
                    repository.Dispose();
                }
                repoPath = Repository.Discover(LiteralPath);
                repository = new Repository(repoPath);
            }
            try
            {
                var blob = repository.Lookup<Blob>(Id);
                if (blob.IsBinary && !ExcludeBinary)
                {
                    WriteObject(blob.Content);
                }
                else
                {
                    
                    WriteObject(blob.ContentAsUtf8());
                }
            }
            catch
            {
                repository.Dispose();
                repository = null;
                repoPath = null;
            }
        }

        protected override void EndProcessing()
        {
            if (repository != null)
            {
                repository.Dispose();
            }
        }
    }
}