namespace PoshGit.Model
{
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;

    using LibGit2Sharp;
    using LibGit2Sharp.Handlers;

    /// <summary>
    /// The git clone helper.
    /// </summary>
    internal static class GitCloneHelper
    {
        /// <summary>
        /// The clone.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="workdirPath">
        /// The working directory path.
        /// </param>
        /// <param name="bare">
        /// true if the clone should be bare, id no working directory.
        /// </param>
        /// <param name="checkout">
        /// Should a working directory be checked out.
        /// </param>
        /// <param name="transferProgress">
        /// The transfer progress.
        /// </param>
        /// <param name="checkoutProgress">
        /// The checkout progress.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        internal static Task Clone(
            string repository, 
            string workdirPath, 
            bool bare, 
            bool checkout, 
            TransferProgressHandler transferProgress, 
            CheckoutProgressHandler checkoutProgress, 
            CancellationToken cancellationToken)
        {
            Contract.Requires(!string.IsNullOrEmpty(repository));
            Contract.Requires(!string.IsNullOrEmpty(workdirPath));
            Contract.Requires(transferProgress != null);
            Contract.Requires(checkoutProgress != null);
            Contract.Requires(cancellationToken != null);

            var done = new AutoResetEvent(false);
            var worker = new Thread(
                () =>
                    {
                        using (
                            Repository.Clone(
                                repository, workdirPath, bare, checkout, transferProgress, checkoutProgress))
                        {
                        }

                        done.Set();
                    });

            var task = Task.Factory.StartNew(
                () =>
                    {
                        worker.Start();
                        WaitHandle.WaitAny(new[] { done, cancellationToken.WaitHandle });
                        cancellationToken.ThrowIfCancellationRequested();
                    },
                cancellationToken);
            task.ContinueWith(worker.Abort, TaskContinuationOptions.NotOnRanToCompletion);
            return task;
        }
    }
}