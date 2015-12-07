using System;

namespace NHibernate.Helper.Exception
{
    /// <summary>
    /// Exception raised when a postcondition fails.
    /// </summary>
    public class PostconditionException : DesignByContractException
    {
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException() { }
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException(string message) : base(message) { }
        /// <summary>
        /// Postcondition Exception.
        /// </summary>
        public PostconditionException(string message, System.Exception inner) : base(message, inner) { }
    }

}
