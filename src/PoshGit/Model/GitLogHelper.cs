using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace PoshGit.Model
{
    internal static class GitLogHelper
    {
        internal static CommitEnumerator EnumerateCommits(string fullname, string reference)
        {
            string repoPath = Repository.Discover(fullname);
            if (String.IsNullOrEmpty(repoPath))
            {
                ExceptionHelper.ThrowInvalidRepositoryPath(fullname);
            }
            var repo = new Repository(repoPath);

            try
            {
                Branch branch = null;
                if (string.IsNullOrEmpty(reference))
                {
                    branch = (from b in repo.Branches
                              where b.IsCurrentRepositoryHead
                              select b).Single();
                }
                else
                {
                    branch = repo.Branches[reference];
                }
                IEnumerable<CommitData> commits = from c in branch.Commits
                                                  select new CommitData(c);
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