namespace PoshGit.Commands
{
    using System.Diagnostics.CodeAnalysis;
    using System.Management.Automation;

    using PoshGit.Model;

    /// <summary>
    /// The PowerShell equivalent of "git rm"
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Reviewed. Suppression is OK here.")]
    [Cmdlet(VerbsCommon.Remove, "GitItem", SupportsShouldProcess = true)]
    public class RemoveGitItemCommand : GitCommandBase
    {
        /// <summary>
        /// Gets or sets the literal path to the items to add.
        /// </summary>
        [Alias("Fullname", "FilePath")]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [HasTabCompleterAttribute]
        public string[] Path { get; set; }

        /// <summary>
        /// Gets or sets the cached.
        /// </summary>
        [Parameter]
        public SwitchParameter Cached { get; set; }

        /// <summary>
        /// The process record.
        /// </summary>
        protected override void ProcessRecord()
        {
            foreach (var p in Path)
            {
                ProviderInfo providerInfo;
                var paths = GetResolvedProviderPathFromPSPath(p, out providerInfo);
                foreach (var resolvedPath in paths)
                {
                    var repo = GitRepositoryFactory.Instance.GetRepository(resolvedPath);
                    var relativePath = resolvedPath.Substring(repo.Info.WorkingDirectory.Length);
                    var action = "Remove-GitItem";
                    if (Cached)
                    {
                        action += " from Index";
                    }

                    if (ShouldProcess(resolvedPath, action))
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
}