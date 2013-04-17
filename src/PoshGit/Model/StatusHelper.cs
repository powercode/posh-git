using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace PoshGit.Model
{
    

    public class StatusHelper
    {
        private readonly RepositoryStatus _repositoryStatus;

        

        private StatusHelper(RepositoryStatus repositoryStatus)
        {
            _repositoryStatus = repositoryStatus;
        }

        public RepositoryStatus Status
        {
            get { return _repositoryStatus; }        
        }        
        
        public bool IsDirty
        {
            get { return _repositoryStatus.IsDirty; }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public static StatusHelper GetStatus(string directory)
        {
            using (var repo = new Repository(Repository.Discover(directory)))
            {
                return new StatusHelper(repo.Index.RetrieveStatus());
            }
        }
    }
}
