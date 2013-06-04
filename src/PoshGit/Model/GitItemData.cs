namespace PoshGit.Model
{
    using System.Diagnostics.Contracts;
    using System.IO;

    using LibGit2Sharp;

    /// <summary>
    ///     The git item data.
    /// </summary>
    public class GitItemData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitItemData"/> class.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        /// <param name="repositoryPath">
        /// The repository path.
        /// </param>
        internal GitItemData(TreeEntry entry, string repositoryPath)
        {
            Contract.Requires(entry != null);
            Contract.Requires(!string.IsNullOrEmpty(repositoryPath));
            RepositoryPath = repositoryPath;
            RelativePath = entry.Path;
            Target = entry.Target;            
            Type = entry.TargetType;
            Mode = entry.Mode;
            Id = Target.Id;
        }

        /// <summary>
        ///     Gets the id.
        /// </summary>
        public ObjectId Id { get; private set; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public TreeEntryTargetType Type { get; private set; }

        /// <summary>
        ///     Gets the mode.
        /// </summary>
        public Mode Mode { get; private set; }

        /// <summary>
        ///     Gets or sets the relative path.
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        ///     Gets the ps path.
        /// </summary>
        public string PSPath
        {
            get
            {
                return Fullname;
            }
        }

        /// <summary>
        ///     Gets the full name.
        /// </summary>
        public string Fullname
        {
            get
            {
                var directoryName = Path.GetDirectoryName(RepositoryPath.TrimEnd(Path.DirectorySeparatorChar));
                Contract.Assert(directoryName != null);
                return Path.Combine(directoryName, RelativePath);
            }
        }

        /// <summary>
        ///     Gets the target.
        /// </summary>
        public GitObject Target { get; private set; }

        /// <summary>
        ///     Gets the repository path.
        /// </summary>
        public string RepositoryPath { get; private set; }
    }
}