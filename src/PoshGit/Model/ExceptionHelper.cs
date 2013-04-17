namespace PoshGit.Model
{
    internal class ExceptionHelper
    {
        public static void ThrowInvalidRepositoryPath(string fullname)
        {
            throw new PathIsNotRepositoryException(fullname);
        }
    }
}