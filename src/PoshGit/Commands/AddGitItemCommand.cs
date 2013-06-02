namespace PoshGit.Commands
{
    using System.Management.Automation;

    using PoshGit.Model;

    /// <summary>
    /// The PowerShell equivalent of "git add".
    /// </summary>
    [Cmdlet(VerbsCommon.Add, "GitItem", SupportsShouldProcess = true)]
    public class AddGitItemCommand : PSCmdlet
    {
        /// <summary>
        /// Gets or sets the literal path to the items to add.
        /// </summary>
        [Alias("Fullname", "FilePath")]
        [Parameter(Mandatory = true, ValueFromPipelineByPropertyName = true, Position = 0)]
        [HasTabCompleterAttribute]
        public string[] Path { get; set; }
        
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
                    if (ShouldProcess(resolvedPath, "Add-GitItem"))
                    {
                        repo.Index.Stage(resolvedPath.Substring(repo.Info.WorkingDirectory.Length));
                    }
                }
            }
        }
    }
}
