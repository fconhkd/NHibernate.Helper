using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PostSharp.Aspects;
using PostSharp.Serialization;

namespace NHibernate.Helper.Aspect
{
    /// <summary>
    /// Aspecto criado para gerenciar as transações do NHibernate.
    /// </summary>
    [Serializable]
    public class TransactionManagementAspect : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Ao entrar assume que já existe uma sessão para o contexto e inicializa a transação.
        /// </summary>
        /// <param name="args"></param>
        public sealed override void OnEntry(MethodExecutionArgs args)
        {
            //var session = Management.SessionManager.Session;
            //if (!session.Transaction.IsActive)
            //    session.BeginTransaction();
            Management.SessionManager.Instance.BeginTransaction();
        }

        /// <summary>
        /// Ao sair verifica o estado da transação e comita se necessario.
        /// </summary>
        /// <param name="args"></param>
        public sealed override void OnExit(MethodExecutionArgs args)
        {
            //var session = Management.SessionManager.Session;
            //if (session.Transaction != null && session.Transaction.IsActive)
            //{
            //    session.Flush();
            //    session.Transaction.Commit();
            //}
            Management.SessionManager.Instance.CommitTransaction();
        }

        /// <summary>
        /// Se ocorrer uma exception verifica o estado da transação e Rollback.
        /// </summary>
        /// <param name="args"></param>
        public sealed override void OnException(MethodExecutionArgs args)
        {
            //var session = Management.SessionManager.Session;
            //if (session.Transaction != null && session.Transaction.IsActive)
            //{
            //    session.Transaction.Rollback();
            //}
            Management.SessionManager.Instance.RollbackTransaction();
        }
    }
}
