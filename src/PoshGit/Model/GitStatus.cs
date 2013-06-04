namespace PoshGit.Model
{
    /// <summary>
    ///     The git status.
    /// </summary>
    public enum GitStatus
    {
        /// <summary>
        /// The none.
        /// </summary>
        None, 

        /// <summary>
        /// The to be committed.
        /// </summary>
        ToBeCommitted, 

        /// <summary>
        /// The not staged for commit.
        /// </summary>
        NotStagedForCommit, 

        /// <summary>
        /// The untracked.
        /// </summary>
        Untracked
    }
}