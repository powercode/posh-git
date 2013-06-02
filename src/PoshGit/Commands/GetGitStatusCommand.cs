namespace PoshGit.Commands
{    
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
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var statusPath = LiteralPath;

            var filterpath = statusPath;

            var repo = GetLiteralPathRepository();            
            using (var statusEnumerator = GitStatusHelper.GetStatusEnumerator(repo, statusPath, filterpath))
            {
                WriteObject(statusEnumerator.Status, true);
            }
        }        
    }
}