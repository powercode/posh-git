namespace PoshGit.Commands
{
    using System.Linq;
    using System.Management.Automation;

    using LibGit2Sharp;

    using PoshGit.Model;

    /// <summary>
    /// The PowerShell equivalent of the "git status" command.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "GitStatus")]
    [OutputType(typeof(StatusEntry))]
    public sealed class GetGitStatusCommand : GitCommandBase
    {
        /// <summary>
        /// Gets or sets whether to include ignored files.
        /// </summary>
        [Parameter]
        public SwitchParameter Ignored { get; set; }

        /// <summary>
        /// Gets or sets the non staged.
        /// </summary>
        [Parameter]
        public SwitchParameter NonStaged { get; set; }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var statusPath = LiteralPath;
            
            var repo = GetLiteralPathRepository();
            var status = GitStatusHelper.GetStatusEnumerator(repo, statusPath);
            if (NonStaged)
            {
                status = status.Where(s => s.State.HasFlag(FileStatus.Modified));
            }

            WriteObject(Ignored ? status : status.Where(s => s.State != FileStatus.Ignored), true);
        }        
    }
}