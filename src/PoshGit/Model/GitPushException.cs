namespace PoshGit.Model
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;

    /// <summary>
    ///     The git push exception.
    /// </summary>
    [Serializable]
    public class GitPushException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitPushException"/> class.
        /// </summary>
        public GitPushException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitPushException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public GitPushException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitPushException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="reference">
        /// The reference.
        /// </param>
        public GitPushException(string message, string reference)
            : base(message)
        {
            this.Reference = reference;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitPushException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <param name="innerException">
        /// The inner exception.
        /// </param>
        public GitPushException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GitPushException"/> class.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        protected GitPushException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires(info != null);
            Reference = info.GetString("reference");
        }

        /// <summary>
        /// The get object data.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("reference", Reference);
        }

        /// <summary>
        /// Gets the reference.
        /// </summary>
        public string Reference { get; private set; }
    }
}