namespace PoshGit.Commands
{
    using System;
    using System.Management.Automation;

    using Signature = LibGit2Sharp.Signature;

    /// <summary>
    /// The submit git item command.
    /// </summary>
    [Cmdlet(VerbsLifecycle.Submit, "GitIndex", SupportsShouldProcess = true)]
    public class SubmitGitIndexCommand : GitCommandBase
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the amend.
        /// </summary>
        [Parameter]
        public SwitchParameter Amend { get; set; }

        /// <summary>
        /// Gets or sets the author.
        /// </summary>
        [Parameter]
        public Signature Author { get; set; }

        /// <summary>
        /// Gets or sets the committer.
        /// </summary>
        [Parameter]
        public Signature Committer { get; set; }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var repo = GetLiteralPathRepository();
            if (Author == null)
            {
                var userName = repo.Config.Get<string>("user.name").Value;
                var userEmail = repo.Config.Get<string>("user.email").Value;

                Author = new Signature(userName, userEmail, DateTimeOffset.Now);
            }

            if (Committer == null)
            {
                Committer = Author;
            }

            if (ShouldProcess("Submit-GitIndex"))
            {
                repo.Commit(Message, Author, Committer, Amend);
            }
        }
    }
}
