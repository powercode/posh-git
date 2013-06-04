namespace PoshGit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using LibGit2Sharp;

    /// <summary>
    /// The git repository factory.
    /// </summary>
    internal class GitRepositoryFactory
    {
        /// <summary>
        /// singleton factory instance.
        /// </summary>
        private static readonly GitRepositoryFactory TheInstance = new GitRepositoryFactory();

        /// <summary>
        /// The repositories.
        /// </summary>
        private readonly Dictionary<string, Repository> repositories = new Dictionary<string, Repository>();

        /// <summary>
        /// Gets the singleton factory instance.
        /// </summary>
        public static GitRepositoryFactory Instance
        {
            get
            {
                return TheInstance;
            }
        }

        /// <summary>
        /// The get repository.
        /// </summary>
        /// <param name="fullPath">
        /// The full path.
        /// </param>
        /// <returns>
        /// The <see cref="Repository"/>.
        /// </returns>
        public static Repository GetRepositoryAtPath(string fullPath)
        {
            Contract.Requires(!string.IsNullOrEmpty(fullPath));
            return Instance.GetRepository(fullPath);
        }

        /// <summary>
        /// The get repository at the given path.
        /// </summary>
        /// <param name="fullPath">
        /// The full path to a repository.
        /// </param>
        /// <returns>
        /// The <see cref="Repository"/>.
        /// </returns>
        public Repository GetRepository(string fullPath)
        {
            Contract.Requires(!string.IsNullOrEmpty(fullPath));
            var repoPath = Repository.Discover(fullPath);
            if (repoPath == null)
            {
                ExceptionHelper.ThrowInvalidRepositoryPath(fullPath);
            }

            Repository repo;
            if (repositories.TryGetValue(repoPath, out repo))
            {
                return repo;
            }

            repo = new Repository(repoPath);
            repositories.Add(repoPath, repo);

            return repo;
        }

        /// <summary>
        /// Disposes all the cached repositories.
        /// </summary>
        public void Clear()
        {
            foreach (var repo in repositories.Values)
            {
                repo.Dispose();
            }

            repositories.Clear();
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(repositories != null);
        }

    }
}