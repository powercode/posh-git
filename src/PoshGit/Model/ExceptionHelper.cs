namespace PoshGit.Model
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// The exception helper.
    /// </summary>
    internal class ExceptionHelper
    {
        /// <summary>
        /// Helper function to throw PathIsNotRepositoryException when a path is not a valid repository path
        /// </summary>
        /// <param name="fullname">
        /// The full name.
        /// </param>
        /// <exception cref="PathIsNotRepositoryException">         
        /// The exception thrown is 
        /// </exception>
        public static void ThrowInvalidRepositoryPath(string fullname)
        {
            Contract.Requires(fullname != null);
            throw new PathIsNotRepositoryException(fullname);
        }
    }
}