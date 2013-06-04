namespace PoshGit.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Management.Automation;

    /// <summary>
    ///     The PowerShell equivalent of "git rm"
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", 
        Justification = "Reviewed. Suppression is OK here.")]
    [Cmdlet(VerbsCommon.Remove, "GitItem", SupportsShouldProcess = true)]
    public class RemoveGitItemCommand : GitCommandBase
    {
        /// <summary>
        ///     Gets or sets the literal path to the items to add.
        /// </summary>
        [Alias("Fullname", "FilePath")]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [HasTabCompleter]
        public string[] Path { get; set; }

        /// <summary>
        ///     Gets or sets the cached.
        /// </summary>
        [Parameter]
        public SwitchParameter Cached { get; set; }

        /// <summary>
        ///     The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            foreach (var path in Path)
            {
                var relativePath = path;
                var repo = GetRepositoryPathRepository();
                if (System.IO.Path.IsPathRooted(relativePath))
                {
                    relativePath = relativePath.Substring(repo.Info.WorkingDirectory.Length);
                }

                var action = "Remove-GitItem";
                if (Cached)
                {
                    action += " from Index";
                }

                if (ShouldProcess(path, action))
                {
                    if (Cached)
                    {
                        repo.Index.Unstage(relativePath);
                    }
                    else
                    {
                        repo.Index.Remove(relativePath);
                    }
                }
            }
        }
    }
}