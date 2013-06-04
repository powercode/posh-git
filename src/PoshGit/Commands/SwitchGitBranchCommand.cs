namespace PoshGit.Commands
{
    using System;
    using System.Management.Automation;

    using LibGit2Sharp;

    /// <summary>
    /// The switch git branch command is the PowerShell equivalent of git checkout &lt;branch&gt;
    /// </summary>
    [Cmdlet(VerbsCommon.Switch, "GitBranch")]
    public class SwitchGitBranchCommand : GitCommandBase
    {
        /// <summary>
        /// Gets or sets the branch to make current.
        /// </summary>
        [Alias("Name")]
        [Parameter(Position = 0, Mandatory = true, ValueFromPipelineByPropertyName = true)]
        [HasTabCompleter]
        public string Branch { get; set; }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            var repo = GetRepositoryPathRepository();
            try
            {
                repo.Checkout(Branch, CheckoutOptions.None, OnProgress);
            }
            catch (MergeConflictException mergeConflict)
            {
                WriteError(new ErrorRecord(mergeConflict, "SwitchBranchConflict", ErrorCategory.ResourceExists, Branch));
            }
        }

        /// <summary>
        /// The on progress.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="completedsteps">
        /// The completed steps.
        /// </param>
        /// <param name="totalsteps">
        /// The total steps.
        /// </param>        
        private void OnProgress(string path, int completedsteps, int totalsteps)
        {
            var percentComplete = completedsteps * 100 / totalsteps;
            var progressRecord = new ProgressRecord(0, "Switch-GitBranch", Branch) { PercentComplete = percentComplete };
            WriteProgress(progressRecord);
        }
    }
}
