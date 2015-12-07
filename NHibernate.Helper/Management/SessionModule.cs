using System;
using System.Web;
using NHibernate;

namespace NHibernate.Helper.Management
{
    /// <summary>
    /// Implementa o padrão Open-Session-In-View usando <see cref="SessionManager" />.
    /// Assume que cada solicitação HTTP é dada uma única transação para todo o ciclo de vida página.
    /// A inspiração para essa classe veio de Ed Courtenay em
    /// http://sourceforge.net/forum/message.php?msg_id=2847509.
    /// </summary>
    public class SessionModule : IHttpModule
    {
        public void Init(HttpApplication context) {
            context.BeginRequest += new EventHandler(BeginTransaction);
            context.EndRequest += new EventHandler(CommitAndCloseSession);
        }

        /// <summary>
        /// Abre uma sessão no inicio do pedido HTTP.
        /// Não abre uma conexão com o banco até que seja necessártio. (Open Session In View)
        /// </summary>
        private void BeginTransaction(object sender, EventArgs e) {
            SessionManager.InitNHibernateSession();
        }

        /// <summary>
        /// Fecha a sessão NHibernate fornecido pelo <see cref="SessionManager"/>.
        /// Assume que a transação foi inicializada pelo <see cref="TransactionManagementAspect"/>
        /// </summary>
        private void CommitAndCloseSession(object sender, EventArgs e) {
            ISession session = SessionManager.Session;
            //fechando e finalizando a sessão do contexto
            session.Close();
            SessionManager.Session = null;
        }

        public void Dispose() { }
    }
}
