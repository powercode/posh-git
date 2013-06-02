// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetGitCommitCommand.cs" company="">
//   
// </copyright>
// <summary>
//   The PowerShell equivalent of git log
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PoshGit.Commands
{    
    using System.Linq;
    using System.Management.Automation;

    using LibGit2Sharp;

    using PoshGit.Model;

    /// <summary>
    ///     The PowerShell equivalent of git log
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "GitLog", SupportsPaging = true)]
    [OutputType(typeof(Commit))]
    public class GetGitLogCommand : GitReferenceCommandBase
    {                
        /// <summary>
        ///     Gets or sets the reference to a commit that limits how far back to get commit entries.
        /// </summary>
        [Parameter]
        public string Until { get; set; }

        /// <summary>
        ///     Gets or sets if the commits should be returned in topological order
        /// </summary>
        [Parameter]
        public SwitchParameter TopologicalOrder { get; set; }

        /// <summary>
        ///     Gets or sets the commits should be returned in reverse order.
        /// </summary>
        [Parameter]
        public SwitchParameter ReverseOrder { get; set; }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {        
            var filter = new Filter { Since = Reference };
            if (TopologicalOrder.IsPresent)
            {
                filter.SortBy |= GitSortOptions.Topological;
            }

            if (ReverseOrder.IsPresent)
            {
                filter.SortBy |= GitSortOptions.Reverse;
            }

            if (MyInvocation.BoundParameters.ContainsKey("Until"))
            {
                filter.Until = Until;
            }

            var takeCount = PagingParameters.First == ulong.MaxValue ? int.MaxValue :
                                            (int)PagingParameters.First;

            var repo = GetLiteralPathRepository();
            var commitLog = repo.Commits.QueryBy(filter);
            var commits = commitLog.AsEnumerable();            
            if (PagingParameters.IncludeTotalCount)
            {
                WriteObject(PagingParameters.NewTotalCount((ulong)commitLog.Count(), 1.0));
            }

            commits = commits.Skip((int)PagingParameters.Skip).Take(takeCount);            

            WriteObject(commits, true);            
        }
    }
}