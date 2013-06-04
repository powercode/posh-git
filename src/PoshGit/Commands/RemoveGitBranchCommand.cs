namespace PoshGit.Commands
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    /// <summary>
    /// The remove git branch command.
    /// </summary>
    [Cmdlet(VerbsCommon.Remove, "GitBranch", SupportsShouldProcess = true)]
    public class RemoveGitBranchCommand : GitCommandBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        [HasTabCompleter]
        public string Name { get; set; }
        
        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var repo = GetRepositoryPathRepository();
            var branch = repo.Branches.FirstOrDefault(b => b.Name == Name);
            if (branch == null)
            {
                WriteError(new ErrorRecord(new ArgumentException("Branch not found.", Name), "RemoveBranchNotFound", ErrorCategory.InvalidArgument, Name));
                return;
            }

            if (ShouldProcess(Name, "Remove-GitBranch"))
            {                            
                repo.Branches.Remove(branch);
            }
        }
    }
}