using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using LibGit2Sharp;
using PoshGit.Model;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Get, "GitItem")]
    [OutputType(typeof (GitItemData))]
    public sealed class GetGitItemCommand : PSCmdlet, IDisposable
    {
        [Alias("PSPath", "RepositoryPath")]
        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true)]
        public string LiteralPath { get; set; }

        [Parameter(Position = 3, ValueFromPipelineByPropertyName = true)]
        public ObjectId Id { get; set; }

        [Parameter(Position = 1)]
        public string[] Include { get; set; }

        [Parameter]
        public string[] Exclude { get; set; }

        private Repository repository;
        private string repositoryPath;
        private WildcardPattern[] includePatterns;
        private WildcardPattern[] excludePatterns;
        

        private bool ShouldWriteItem(GitItemData item)
        {
            if ((excludePatterns != null && excludePatterns.Any(p => p.IsMatch(item.Fullname))))
            {
                return false;
            }
            if (includePatterns != null && !includePatterns.Any(p => p.IsMatch(item.Fullname)))
            {
                return false;
            }

            return true;
        }


        protected override void BeginProcessing()
        {
            if (Include != null)
            {
                includePatterns = CreatePatterns(Include);
            }
            if (Exclude != null)
            {
                excludePatterns = CreatePatterns(Exclude);
            }
            if (string.IsNullOrEmpty(LiteralPath))
            {
                LiteralPath = SessionState.Path.CurrentFileSystemLocation.ProviderPath;
            }
            repositoryPath = Repository.Discover(LiteralPath);
            if (repositoryPath == null){
                ExceptionHelper.ThrowInvalidRepositoryPath(LiteralPath);
            }
            repository = new Repository(repositoryPath);
        }

        private static WildcardPattern[] CreatePatterns(IEnumerable<string> pattern)
        {
            return (from p in pattern
                    select new WildcardPattern(p, WildcardOptions.IgnoreCase)).ToArray();
        }

        protected override void EndProcessing()
        {
            if (repository != null)
            {
                repository.Dispose();
            }
        }

        protected override void ProcessRecord()
        {
            try
            {
                
                var commit = repository.Lookup<Commit>(Id);
                var parent = commit.Parents.FirstOrDefault();
                {
                    var parentTree = parent == null ? null : parent.Tree;
                    var changes = repository.Diff.Compare(parentTree, commit.Tree);
                    WriteObject(changes, true);
                }
                

            }
            catch
            {
                repository.Dispose();
                repository = null;
            }
        }
        
        public void Dispose()
        {
            if (repository != null)
            {
                repository.Dispose();
            }
        }        
    }
}