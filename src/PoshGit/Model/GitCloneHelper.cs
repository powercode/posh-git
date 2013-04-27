using System.Threading;
using System.Threading.Tasks;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace PoshGit.Model
{
    internal static class GitCloneHelper
    {
        internal static Task Clone(string repository, string workdirPath, bool bare, bool checkout, TransferProgressHandler transferProgress, CheckoutProgressHandler checkoutProgress, CancellationToken cancellationToken)
        {
            var done = new AutoResetEvent(false);
            var worker = new Thread(() =>
                {
                    using (
                        Repository.Clone(repository, workdirPath, bare, checkout, transferProgress,
                                         checkoutProgress))
                    {
                    }
                    done.Set();
                });

            var task = Task.Factory.StartNew(() =>
                {
                    worker.Start();
                    WaitHandle.WaitAny(new[] {done, cancellationToken.WaitHandle});
                    cancellationToken.ThrowIfCancellationRequested();
                }, cancellationToken);
            task.ContinueWith(worker.Abort, TaskContinuationOptions.NotOnRanToCompletion);
            return task;
        }    
    }
}