namespace PoshGit.Commands
{
    using System.Management.Automation;

    /// <summary>
    /// The git command base.
    /// </summary>
    public class GitReferenceCommandBase : GitCommandBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitReferenceCommandBase"/> class.
        /// </summary>
        protected GitReferenceCommandBase()
        {
            Reference = "HEAD";
        }

        /// <summary>
        ///     Gets or sets the reference used by git commands.
        /// </summary>        
        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true)]
        public string Reference { get; set; }
    }
}