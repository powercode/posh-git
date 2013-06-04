namespace PoshGit.Commands
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Management.Automation;

    using LibGit2Sharp;

    /// <summary>
    /// The update git branch command.
    /// </summary>
    [Cmdlet(VerbsData.Update, "GitBranch")]
    public class UpdateGitBranchCommand : GitReferenceCommandBase
    {
        /// <summary>
        ///     Gets or sets the remote.
        /// </summary>
        [Parameter(Position = 1)]
        [HasTabCompleter]
        public string Remote { get; set; }

        /// <summary>
        ///     Gets or sets the reference.
        /// </summary>
        [Parameter(Position = 2)]
        [HasTabCompleter]
        public new string[] Reference { get; set; }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var repo = GetRepositoryPathRepository();
            var remote = repo.Network.Remotes.First(r => r.Name == Remote);

            repo.Network.Fetch(remote, TagFetchMode.Auto, OnProgress, null, OnUpdateTips);
        }

        /// <summary>
        /// The on update tips.
        /// </summary>
        /// <param name="referencename">
        /// The reference name.
        /// </param>
        /// <param name="oldid">
        /// The old id.
        /// </param>
        /// <param name="newid">
        /// The new id.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int OnUpdateTips(string referencename, ObjectId oldid, ObjectId newid)
        {
            Contract.Requires(oldid != null);
            Contract.Requires(newid != null);
            Host.UI.WriteLine(oldid.Sha + " => " + newid.Sha);
            return 0;
        }

        /// <summary>
        /// The on progress.
        /// </summary>
        /// <param name="serverprogressoutput">
        /// The server progress output.
        /// </param>
        private void OnProgress(string serverprogressoutput)
        {
            Host.UI.WriteLine(serverprogressoutput);
        }
    }
}