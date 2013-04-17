using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace PoshGit.Model
{
    internal static class GitStatusHelper
    {
        internal static StatusEnumerator GetStatusEnumerator(string fullname, string filterPath)
        {
            string repoPath = Repository.Discover(fullname);
            if (String.IsNullOrEmpty(repoPath))
            {
                ExceptionHelper.ThrowInvalidRepositoryPath(fullname);
            }
            var repo = new Repository(repoPath);
            try
            {
                IEnumerable<StatusEntry> status;
                if (fullname == filterPath || String.IsNullOrEmpty(filterPath))
                {
                    status = repo.Index.RetrieveStatus().OrderBy(GitIndexStatusHelper.Status);
                }
                else
                {
                    var rel = filterPath.Substring(repoPath.Length - 5);
                    status = repo.Index.RetrieveStatus()
                        .OrderBy(GitIndexStatusHelper.Status)
                        .Where(fs => fs.FilePath.StartsWith(rel, StringComparison.OrdinalIgnoreCase));
                }
                return new StatusEnumerator(repo, status);
            }
            catch
            {
                repo.Dispose();
                throw;
            }
        }        
    }

    internal class StatusEnumerator : IDisposable
    {
        private readonly Repository repo;
        private readonly IEnumerable<StatusEntry> status;

        public StatusEnumerator(Repository repo, IEnumerable<StatusEntry> status)
        {
            this.repo = repo;
            this.status = status;            
        }


        public IEnumerable<StatusEntry> Status
            {
                get { return status; }
            }

            void IDisposable.Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~StatusEnumerator()
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