using System;
using PoshGit.Properties;

namespace PoshGit.Model
{
    [Serializable]
    public class PathIsNotRepositoryException : Exception
    {
        public PathIsNotRepositoryException(string fullname) : base(ResourceStrings.Format(Resources.PathIsNotARepo, fullname))
        {
            
        }
    }
}