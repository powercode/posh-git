namespace PoshGit.Commands
{
    using System.Management.Automation;

    /// <summary>
    /// The get_ git repository status command.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "GitRepositoryStatus")]
    public class GetGitRepositoryStatusCommand : GitCommandBase
    {
        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var repo = GetRepositoryPathRepository();

            var repositoryStatus = repo.Index.RetrieveStatus();  
                     
            WriteObject(repositoryStatus, false);
        }
    }
}
