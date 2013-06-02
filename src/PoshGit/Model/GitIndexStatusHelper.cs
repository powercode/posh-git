namespace PoshGit.Model
{
    using System.Diagnostics.Contracts;
    using System.Management.Automation;

    using LibGit2Sharp;

    /// <summary>
    /// The git index status helper.
    /// </summary>
    public class GitIndexStatusHelper
    {
        /// <summary>
        /// Gets the git status of a PSObject.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <returns>
        /// The <see cref="GitStatus"/>.
        /// </returns>
        public static GitStatus Status(PSObject instance)
        {
            if (instance == null)
            {
                return GitStatus.None;
            }

            var entry = (StatusEntry)instance.BaseObject;
            return Status(entry);
        }

        /// <summary>
        /// Converts a StatusEntry to a GitStatus
        /// </summary>
        /// <param name="statusEntry">
        /// The status entry.
        /// </param>
        /// <returns>
        /// The <see cref="GitStatus"/>.
        /// </returns>
        internal static GitStatus Status(StatusEntry statusEntry)
        {
            Contract.Requires(statusEntry != null);            
            var state = statusEntry.State;
            if (state.HasFlag(FileStatus.Added) || state.HasFlag(FileStatus.Staged))
            {
                return GitStatus.ToBeCommitted;
            }

            return state.HasFlag(FileStatus.Modified) ? GitStatus.NotStagedForCommit : GitStatus.Untracked;
        }
    }
}