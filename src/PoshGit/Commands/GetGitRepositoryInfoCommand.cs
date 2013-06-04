namespace PoshGit.Commands
{
    using System.Management.Automation;

    /// <summary>
    /// The get git repository info command.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "GitRepositoryInfo")]
    public class GetGitRepositoryInfoCommand : GitCommandBase
    {
        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var repo = GetRepositoryPathRepository();            
            WriteObject(repo.Info);
        }       
    }
}