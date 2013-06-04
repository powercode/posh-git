namespace PoshGit.Commands
{
    using System.Linq;
    using System.Management.Automation;

    using LibGit2Sharp;

    /// <summary>
    /// The new git branch command.
    /// </summary>
    [Cmdlet(VerbsCommon.New, "GitBranch", SupportsShouldProcess = true)]
    public class NewGitBranchCommand : GitCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewGitBranchCommand"/> class.
        /// </summary>
        public NewGitBranchCommand()
        {
            Reference = "HEAD";
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Parameter(Mandatory = true, Position = 0)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the reference.
        /// </summary>
        [Parameter(Mandatory = true, Position = 1)]
        [HasTabCompleter]
        public string Reference { get; set; }

        /// <summary>
        /// Gets or sets the Force, indicating a branch will be created even if one already exists.
        /// </summary>
        [Parameter]
        public SwitchParameter Force { get; set; }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {            
            var repo = GetRepositoryPathRepository();
            var commit = repo.Head.Tip;
            if (!string.IsNullOrEmpty(Reference))
            {
                commit = repo.Commits.QueryBy(new Filter { Since = Reference }).First();
            }

            if (ShouldProcess(Name + " => " + Reference, "New-GitBranch"))
            {
                repo.Branches.Add(Name, commit, Force);
            }
        }
    }
}
