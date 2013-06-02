using System.IO;
using LibGit2Sharp;

namespace PoshGit.Model
{
    using System;
    using System.Diagnostics.Contracts;

    public class GitItemData
    {
        internal GitItemData(TreeEntry entry, string repositoryPath)
        {
            Contract.Requires(entry != null);
            Contract.Requires(!String.IsNullOrEmpty(repositoryPath));
            RepositoryPath = repositoryPath;
            RelativePath = entry.Path;
            Target = entry.Target;
            Type = entry.Type;
            Mode = entry.Mode;
            Id = Target.Id;
        }

        public ObjectId Id { get; private set; }
        public GitObjectType Type { get; private set; }
        public Mode Mode { get; private set; }
        public string RelativePath { get; set; }
        public string PSPath { get { return Fullname; } }
        public string Fullname
        {
            get
            {
                var directoryName = Path.GetDirectoryName(RepositoryPath.TrimEnd(Path.DirectorySeparatorChar));
                Contract.Assert(directoryName != null);
                return Path.Combine(directoryName, RelativePath);
            }
        }
        public GitObject Target { get; private set; }


        public string RepositoryPath { get; private set; }

    }
}