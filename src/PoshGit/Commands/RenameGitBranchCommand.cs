namespace PoshGit.Commands
{
    using System;
    using System.Linq;
    using System.Management.Automation;

    /// <summary>
    /// The rename git branch command.
    /// </summary>
    [Cmdlet(VerbsCommon.Rename, "GitBranch", SupportsShouldProcess = true)]
    public class RenameGitBranchCommand : GitCommandBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        [HasTabCompleter]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the new name.
        /// </summary>
        [Parameter(Mandatory = true, Position = 1)]
        public string NewName { get; set; }

        /// <summary>
        /// Gets or sets weather to force rename even if a branch with the new name exists.
        /// </summary>
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var repo = GetRepositoryPathRepository();
            var branch = repo.Branches.FirstOrDefault(b => b.Name == Name);
            if (branch == null)
            {
                WriteError(new ErrorRecord(new ArgumentException("Branch not found.", Name), "RenameBranchNotFound", ErrorCategory.InvalidArgument, Name));
                return;
            }
            
            if (ShouldProcess(Name, "Rename-Branch"))
            {                
                repo.Branches.Move(branch, NewName, Force);
            }            
        }
    }
}