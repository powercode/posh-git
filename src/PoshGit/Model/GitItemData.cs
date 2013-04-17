using System.IO;
using LibGit2Sharp;

namespace PoshGit.Model
{
    public class GitItemData
    {
        internal GitItemData(TreeEntry entry, string repositoryPath)
        {
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
            get { return Path.Combine(Path.GetDirectoryName(RepositoryPath.TrimEnd(Path.DirectorySeparatorChar)), RelativePath); }
        }
        public GitObject Target { get; private set; }


        public string RepositoryPath { get; private set; }
    }
}