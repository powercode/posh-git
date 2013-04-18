using System;
using System.Runtime.Serialization;
using PoshGit.Properties;

namespace PoshGit.Model
{
    [Serializable]
    public class PathIsNotRepositoryException : Exception
    {
        public PathIsNotRepositoryException(string fullname) : base(ResourceStrings.Format(Strings.PathIsNotARepoFormat_Path, fullname))
        {
            
        }

        public PathIsNotRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PathIsNotRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public PathIsNotRepositoryException()
        {
        }
    }
}