using System;
using System.Management.Automation;
using LibGit2Sharp;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Get, "GitContent")]
    [OutputType(typeof (string))]
    [OutputType(typeof (byte[]))]
    public sealed class GetGitContentCommand : PSCmdlet, IDisposable
    {[Alias("PSPath", "RepositoryPath")]
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
        

        ~GetGitContentCommand()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        private void Dispose(bool disposed)
        {
            if (disposed)
            {
                repository.Dispose();
                repository = null;
            }
        }
    }
}