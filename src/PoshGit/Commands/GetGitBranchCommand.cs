namespace PoshGit.Commands
{
    using System.Linq;
    using System.Management.Automation;

    using LibGit2Sharp;

    using PoshGit.Model;

    /// <summary>
    /// The get git branch command.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "GitBranch")]
    [OutputType(typeof(Branch))]
    public class GetGitBranchCommand : GitCommandBase
    {
        /// <summary>
        /// Gets or sets the all switch.
        /// </summary>
        [Parameter]
        public SwitchParameter All { get; set; }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {            
            var repo = GetLiteralPathRepository();
            WriteObject(All ? repo.Branches : repo.Branches.Where(b => b.IsRemote == false), true);            
        }
    }
}