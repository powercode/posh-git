using System.Management.Automation;
using LibGit2Sharp;

namespace PoshGit.Model
{
    public class GitIndexStatusHelper
    {
        internal static GitStatus Status(StatusEntry statusEntry)
        {
            var state = statusEntry.State;
            if ((state.HasFlag(FileStatus.Added) || state.HasFlag(FileStatus.Staged)) && !state.HasFlag(FileStatus.Modified))
            {
                return GitStatus.ToBeCommitted;
            }
            if (state.HasFlag(FileStatus.Modified))
            {
                return GitStatus.NotStagedForCommit;
            }
            return GitStatus.Untracked;            
        }

        public static GitStatus Status(PSObject instance)
        {
            if (instance == null)
                return GitStatus.None;
            var entry = (StatusEntry)instance.BaseObject;
            return Status(entry);
        }
    }
}