namespace PoshGit.Commands
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Management.Automation;
    using System.Threading;

    using LibGit2Sharp;

    using PoshGit.Model;
    using PoshGit.Properties;

    /// <summary>
    /// The PowerShell representation of git clone.
    /// </summary>
    [Cmdlet(VerbsCommon.Copy, "GitRepository", DefaultParameterSetName = "Path")]
    [OutputType(typeof(DirectoryInfo))]
    public sealed class CopyGitRepositoryCommand : PSCmdlet, IDisposable
    {
        /// <summary>
        /// The cancellation token source.
        /// </summary>
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// The progress collection.
        /// </summary>
        private readonly BlockingCollection<ProgressRecord> progressCollection =
            new BlockingCollection<ProgressRecord>();

        /// <summary>
        /// The checkout percent complete.
        /// </summary>
        private int checkoutPercentComplete;

        /// <summary>
        /// The transfer percent complete.
        /// </summary>
        private int transferPercentComplete;

        /// <summary>
        /// Finalizes an instance of the <see cref="CopyGitRepositoryCommand"/> class. 
        /// </summary>
        ~CopyGitRepositoryCommand()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets or sets the source path.
        /// </summary>
        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, 
            ParameterSetName = "Path")]
        public string SourcePath { get; set; }

        /// <summary>
        /// Gets or sets the source uri.
        /// </summary>
        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, 
            ParameterSetName = "Uri")]
        public Uri SourceUri { get; set; }

        /// <summary>
        /// Gets or sets the literal path.
        /// </summary>
        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true)]
        public string LiteralPath { get; set; }

        /// <summary>
        /// Gets or sets the bare.
        /// </summary>
        [Parameter]
        public SwitchParameter Bare { get; set; }

        /// <summary>
        /// Gets or sets the no checkout.
        /// </summary>
        [Parameter]
        public SwitchParameter NoCheckout { get; set; }

        /// <summary>
        /// The dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Called by PowerShell when the user aborts a command.
        /// </summary>
        protected override void StopProcessing()
        {
            cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// The process record.
        /// </summary>
        /// <exception cref="Exception">
        /// </exception>
        protected override void ProcessRecord()
        {
            if (ParameterSetName == "Path")
            {
                SourceUri = new Uri(GetUnresolvedProviderPathFromPSPath(SourcePath));
            }

            if (string.IsNullOrEmpty(LiteralPath))
            {
                LiteralPath = Path.Combine(
                    SessionState.Path.CurrentFileSystemLocation.ProviderPath, Path.GetFileName(SourceUri.AbsoluteUri));
            }
            else
            {
                LiteralPath = GetUnresolvedProviderPathFromPSPath(LiteralPath);
            }

            var task = GitCloneHelper.Clone(
                SourceUri.AbsoluteUri, 
                LiteralPath, 
                Bare.IsPresent, 
                !NoCheckout.IsPresent, 
                OnTransferProgress, 
                OnCheckoutProgress, 
                cancellationTokenSource.Token);
            task.ContinueWith(c => progressCollection.CompleteAdding());

            foreach (var pr in progressCollection.GetConsumingEnumerable(cancellationTokenSource.Token))
            {
                WriteProgress(pr);
            }

            if (task.Exception != null)
            {
                throw task.Exception.GetBaseException();
            }
        }

        /// <summary>
        /// The on checkout progress.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="completedSteps">
        /// The completed steps.
        /// </param>
        /// <param name="totalSteps">
        /// The total steps.
        /// </param>
        private void OnCheckoutProgress(string path, int completedSteps, int totalSteps)
        {
            var newPercentComplete = completedSteps * 100 / totalSteps;
            if (newPercentComplete == checkoutPercentComplete)
            {
                return;
            }

            checkoutPercentComplete = newPercentComplete;
            var activity = ResourceStrings.Format(Strings.CheckoutProgressActivityFormat_Local, LiteralPath);
            var status = ResourceStrings.Format(
                Strings.CheckoutProgressStatusFormat_Completed_Total, completedSteps, totalSteps);
            var progressRecord = new ProgressRecord(2, activity, status) { PercentComplete = checkoutPercentComplete };
            progressCollection.Add(progressRecord);
        }

        /// <summary>
        /// The on transfer progress.
        /// </summary>
        /// <param name="progress">
        /// The progress.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int OnTransferProgress(TransferProgress progress)
        {
            Contract.Requires(progress != null);
            var percentComplete = progress.ReceivedObjects * 100 / progress.TotalObjects;
            if (percentComplete == transferPercentComplete)
            {
                return Stopping ? -1 : 0;
            }

            transferPercentComplete = percentComplete;
            var activity = ResourceStrings.Format(Strings.TransferProgressActivityFormat_Source, SourceUri);
            var status = ResourceStrings.Format(
                Strings.TransferProgressStatusFormat_completed_total, progress.ReceivedObjects, progress.TotalObjects);
            var bytesReceived = ResourceStrings.Format(
                Strings.TransferProgressCurrentOperation_Format, progress.ReceivedBytes / (1024 * 1024));
            var progressRecord = new ProgressRecord(2, activity, status)
                                     {
                                         PercentComplete = percentComplete, 
                                         CurrentOperation = bytesReceived
                                     };

            progressCollection.Add(progressRecord);
            if (Stopping)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposed">
        /// The disposed.
        /// </param>
        private void Dispose(bool disposed)
        {
            if (!disposed)
            {
                return;
            }

            progressCollection.Dispose();
            cancellationTokenSource.Dispose();
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(cancellationTokenSource != null);
            Contract.Invariant(progressCollection != null);
        }

    }
}