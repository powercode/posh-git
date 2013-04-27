using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace PoshGit.Model
{
    internal static class GitCommitHelper
    {
        internal static CommitEnumerator EnumerateCommits(string repository, string reference, Filter filter)
        {
            var repoPath = Repository.Discover(repository);
            if (String.IsNullOrEmpty(repoPath))
            {
                ExceptionHelper.ThrowInvalidRepositoryPath(repository);
            }
            var repo = new Repository(repoPath);

            try
            {  
                if (String.IsNullOrEmpty(reference))
                {
                    var branch = (from b in repo.Branches
                              where b.IsCurrentRepositoryHead
                              select b).Single();
                    filter.Since = branch.Tip;
                }
                var commits = from c in repo.Commits.QueryBy(filter)
                                select new CommitData(c, repoPath);
                return new CommitEnumerator(repo, commits);                
            }
            catch
            {
                repo.Dispose();
                throw;
            }
        }

        internal sealed class CommitEnumerator : IDisposable
        {
            private readonly IEnumerable<CommitData> enumerable;
            private readonly Repository repo;

            internal CommitEnumerator(Repository repo, IEnumerable<CommitData> enumerable)
            {
                this.repo = repo;
                this.enumerable = enumerable;
            }

            public IEnumerable<CommitData> Commits
            {
                get { return enumerable; }
            }

            void IDisposable.Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~CommitEnumerator()
            {
                Dispose(false);
            }

            private void Dispose(bool disposed)
            {
                if (disposed)
                {
                    repo.Dispose();
                }
            }
        }
    }
}