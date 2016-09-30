using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PostSharp.Aspects;

namespace NHibernate.Helper.Aspect
{
    [Serializable]
    public class SessionManagementAspect : OnMethodBoundaryAspect
    {
        /// <summary>
        /// Ao entrar assume que já existe uma sessão para o contexto e inicializa a transação.
        /// </summary>
        /// <param name="args"></param>
        public sealed override void OnEntry(MethodExecutionArgs args)
        {
            //var session = Management.SessionManager.GetCurrentSession();
        }

        /// <summary>
        /// Ao sair verifica o estado da transação e comita se necessario.
        /// </summary>
        /// <param name="args"></param>
        public sealed override void OnExit(MethodExecutionArgs args)
        {
            //var session = Management.SessionManager.Session;
            //session.Close();
            //Management.SessionManager.Session = null;
        }

        ///// <summary>
        ///// Se ocorrer uma exception verifica o estado da transação e Rollback.
        ///// </summary>
        ///// <param name="args"></param>
        public sealed override void OnException(MethodExecutionArgs args)
        {
            Management.SessionManager.Instance.CloseSession();
        }
    }
}
