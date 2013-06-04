namespace PoshGit.Commands
{
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

        private int OnUpdateTips(string referencename, ObjectId oldid, ObjectId newid)
        {
            Host.UI.Write(oldid.Sha + " => " + newid.Sha);
            return 0;
        }

        private void OnProgress(string serverprogressoutput)
        {
            Host.UI.Write(serverprogressoutput);
        }
    }
}