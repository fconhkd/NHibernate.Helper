using System;
using NHibernate;

namespace NHibernate.Helper.Management
{
    public class Filter : System.Web.IHttpModule
    {
        public void Init(System.Web.HttpApplication context)
        {
            context.BeginRequest += new EventHandler(BeginTransaction);
            context.EndRequest += new EventHandler(CommitAndCloseSession);
        }

        /// <summary>
        /// Opens a session within a transaction at the beginning of the HTTP request.
        /// This doesn't actually open a connection to the database until needed.
        /// </summary>
        private void BeginTransaction(object sender, EventArgs e)
        {
            SessionManager.InitNHibernateSession();
        }

        /// <summary>
        /// Commits and closes the NHibernate session provided by the supplied <see cref="NHibernateSessionManager"/>.
        /// Assumes a transaction was begun at the beginning of the request; but a transaction or session does
        /// not *have* to be opened for this to operate successfully.
        /// </summary>
        private void CommitAndCloseSession(object sender, EventArgs e)
        {
            ISession session = SessionManager.Session;
            session.Flush();
            if (session.Transaction != null && session.Transaction.IsActive)
                session.Transaction.Commit();
            session.Close();
            SessionManager.Session = null;
        }

        public void Dispose() { }
    }
}
