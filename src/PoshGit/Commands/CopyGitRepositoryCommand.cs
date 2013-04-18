using System;
using System.Collections.Concurrent;
using System.IO;
using System.Management.Automation;
using System.Threading;
using LibGit2Sharp;
using PoshGit.Model;
using PoshGit.Properties;

namespace PoshGit.Commands
{
    [Cmdlet(VerbsCommon.Copy, "GitRepository", DefaultParameterSetName = "Path")]
    [OutputType(typeof (DirectoryInfo))]
    public class CopyGitRepositoryCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Path")]        
        public string SourcePath { get; set; }

        [Parameter(Mandatory = true, Position = 1, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Uri")]        
        public Uri SourceUri { get; set; }
                
        [Parameter(Position = 2, ValueFromPipelineByPropertyName = true)]
        public string LiteralPath { get; set; }

        [Parameter]
        public SwitchParameter Bare { get; set; }

        [Parameter]
        public SwitchParameter NoCheckout { get; set; }

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly BlockingCollection<ProgressRecord> progressCollection =
            new BlockingCollection<ProgressRecord>();

        protected override void StopProcessing()
        {
            cancellationTokenSource.Cancel();
        }

        protected override void ProcessRecord()
        {
            if (ParameterSetName == "Path")
            {
                SourceUri = new Uri(GetUnresolvedProviderPathFromPSPath(SourcePath));
            }

            if (string.IsNullOrEmpty(LiteralPath))
            {
                LiteralPath = Path.Combine(SessionState.Path.CurrentFileSystemLocation.ProviderPath,
                                           Path.GetFileName(SourceUri.AbsoluteUri));
            }
            else
            {
                LiteralPath = GetUnresolvedProviderPathFromPSPath(LiteralPath);
            }

            var task = GitCloneHelper.Clone(SourceUri.AbsoluteUri, LiteralPath, Bare.IsPresent, !NoCheckout.IsPresent,
                                            OnTransferProgress, OnCheckoutProgress, cancellationTokenSource.Token);
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


        private void OnCheckoutProgress(string path, int completedsteps, int totalsteps)
        {
            var percentComplete = completedsteps*100/totalsteps;
            var activity = ResourceStrings.Format(Strings.CheckoutProgressActivityFormat_Local, LiteralPath);
            var status = ResourceStrings.Format(Strings.CheckoutProgressStatusFormat_Completed_Total, completedsteps,
                                                   totalsteps);
            var progressRecord = new ProgressRecord(2, activity, status) {PercentComplete = percentComplete};
            progressCollection.Add(progressRecord);
        }

        private int OnTransferProgress(TransferProgress progress)
        {
            var percentComplete = progress.ReceivedObjects*100/progress.TotalObjects;
            var activity = ResourceStrings.Format(Strings.TransferProgressActivityFormat_Source, SourceUri);
            var status = ResourceStrings.Format(Strings.TransferProgressStatusFormat_completed_total,
                                                progress.ReceivedObjects, progress.TotalObjects);
            var bytesReceived = ResourceStrings.Format(Strings.TransferProgressCurrentOperation_Format,
                                                       progress.ReceivedBytes/(1024*1024));
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
    }
}