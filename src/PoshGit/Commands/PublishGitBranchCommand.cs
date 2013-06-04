namespace PoshGit.Commands
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Management.Automation;

    using LibGit2Sharp;

    using PoshGit.Model;

    /// <summary>
    ///     The publish git branch command ("git push").
    /// </summary>
    [Cmdlet(VerbsData.Publish, "GitBranch")]
    public class PublishGitBranchCommand : GitReferenceCommandBase
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
        ///     Gets or sets the credentials to use.
        /// </summary>
        [Parameter]
        public PSCredential Credentials { get; set; }

        /// <summary>
        ///     The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            Credentials credentials = null;
            if (Credentials != null)
            {
                credentials = new Credentials
                                  {
                                      Username = Credentials.UserName, 
                                      Password = Credentials.GetNetworkCredential().Password
                                  };
            }

            var repo = GetRepositoryPathRepository();
            var remote = repo.Network.Remotes.First(r => r.Name == Remote);
            if (Reference == null)
            {
                Reference = (from b in repo.Branches where b.IsCurrentRepositoryHead select b.Tip.Sha).ToArray();
            }

            if (ShouldProcess(string.Join(", ", Reference), "Publish-GitBranch"))
            {
                repo.Network.Push(remote, Reference, OnPushStatusError, credentials);
            }
        }

        /// <summary>
        /// The on push status error.
        /// </summary>
        /// <param name="pushstatuserrors">
        /// The push status errors.
        /// </param>
        private void OnPushStatusError(PushStatusError pushstatuserrors)
        {
            Contract.Requires(pushstatuserrors != null);
            var ex = new GitPushException(pushstatuserrors.Message, pushstatuserrors.Reference);
            WriteError(new ErrorRecord(ex, "GitPushError", ErrorCategory.NotSpecified, pushstatuserrors.Reference));
        }
    }
}