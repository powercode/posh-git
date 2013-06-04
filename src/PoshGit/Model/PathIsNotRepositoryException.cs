namespace PoshGit.Model
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;

    using PoshGit.Properties;

    /// <summary>
    /// The path is not repository exception.
    /// </summary>
    [Serializable]
    public class PathIsNotRepositoryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PathIsNotRepositoryException"/> class.
        /// </summary>
        /// <param name="fullname">
        /// The full name.
        /// </param>
        public PathIsNotRepositoryException(string fullname) : base(ResourceStrings.Format(Strings.PathIsNotARepoFormat_Path, fullname))
        {
            Contract.Requires(!string.IsNullOrEmpty(fullname));    
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathIsNotRepositoryException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public PathIsNotRepositoryException(string message, Exception innerException) : base(message, innerException)
        {
            Contract.Requires(!string.IsNullOrEmpty(message));
            Contract.Requires(innerException != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathIsNotRepositoryException"/> class.
        /// </summary>
        public PathIsNotRepositoryException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathIsNotRepositoryException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected PathIsNotRepositoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}