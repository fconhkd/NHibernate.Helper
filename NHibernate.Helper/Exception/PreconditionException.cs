using System;

namespace NHibernate.Helper.Exception
{
    /// <summary>
    /// Exception raised when a precondition fails.
    /// </summary>
    public class PreconditionException : DesignByContractException
    {
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException() { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message) : base(message) { }
        /// <summary>
        /// Precondition Exception.
        /// </summary>
        public PreconditionException(string message, System.Exception inner) : base(message, inner) { }
    }

}
