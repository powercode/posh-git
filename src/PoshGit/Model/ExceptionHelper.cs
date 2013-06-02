namespace PoshGit.Model
{
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
        /// </exception>
        public static void ThrowInvalidRepositoryPath(string fullname)
        {
            throw new PathIsNotRepositoryException(fullname);
        }
    }
}