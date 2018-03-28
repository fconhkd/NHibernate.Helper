using System;
using System.Web;
using System.Runtime.Remoting.Messaging;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Helper.Management
{
    [Serializable]
    public sealed class SessionManager
    {
        //private static ISession _session; //this session is not used in web
        private static ISessionFactory _sessionFactory = null;
        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
        private const string SESSION_KEY = "CONTEXT_SESSION";

        #region Thread-safe, lazy Singleton
        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static SessionManager Instance
        {
            get
            {
                return Nested.NHibernateSessionManager;
            }
        }

        /// <summary>
        /// Initializes the NHibernate session factory upon instantiation.
        /// </summary>
        private SessionManager()
        {
            InitializeSessionFactory();
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            static Nested() { }
            internal static readonly SessionManager NHibernateSessionManager =
                new SessionManager();
        }
        #endregion



        /// <summary>
        /// Retorna a SessionFactory do NHibernate
        /// </summary>
        /// <returns>ISessionFactory</returns>
        public ISessionFactory GetSessionFactory()
        {
            InitializeSessionFactory();
            return _sessionFactory;
        }

        /// <summary>
        /// Retorna uma nova Session para ser usado na aplicação
        /// </summary>
        /// <returns>ISession</returns>
        public ISession GetSession()
        {
            return GetSession(null);
        }

        /// <summary>
        /// Cria e inicializa a sessão para ser usada no contexto.
        /// </summary>
        public void OpenSession()
        {
            //Seta uma sessão nova para ser usada.
            Session = GetSession();
        }

        public ISession GetSession(IInterceptor interceptor)
        {
            ISession session = Session;
            if (session == null)
            {
                if (interceptor != null)
                {
                    session = GetSessionFactory().WithOptions().Interceptor(interceptor).OpenSession();// OpenSession(interceptor);
                }
                else
                {
                    session = GetSessionFactory().OpenSession();
                }
                Session = session;
            }
            return session;
        }

        /// <summary>
        /// Flushes anything left in the session and closes the connection.
        /// </summary>
        public void CloseSession()
        {
            ISession session = Session;
            if (session != null && session.IsOpen)
            {
                session.Flush();
                session.Close();
            }
            Session = null;
        }

        public void BeginTransaction()
        {
            ITransaction transaction = ContextTransaction;

            if (transaction == null)
            {
                transaction = GetSession().BeginTransaction();
                ContextTransaction = transaction;
            }
        }

        public void CommitTransaction()
        {
            ITransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction())
                {
                    transaction.Commit();
                    ContextTransaction = null;
                }
            }
            catch (HibernateException)
            {
                RollbackTransaction();
                throw;
            }
        }

        public bool HasOpenTransaction()
        {
            ITransaction transaction = ContextTransaction;

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        public void RollbackTransaction()
        {
            ITransaction transaction = ContextTransaction;

            try
            {
                if (HasOpenTransaction())
                {
                    transaction.Rollback();
                }

                ContextTransaction = null;
            }
            finally
            {
                CloseSession();
            }
        }

        /// <summary>
        /// If within a web context, this uses <see cref="HttpContext" /> instead of the WinForms 
        /// specific <see cref="CallContext" />.  Discussion concerning this found at 
        /// http://forum.springframework.net/showthread.php?t=572.
        /// </summary>
        private ITransaction ContextTransaction
        {
            get
            {
                if (IsInWebContext())
                {
                    return (ITransaction)HttpContext.Current.Items[TRANSACTION_KEY];
                }
                else
                {
                    return (ITransaction)CallContext.GetData(TRANSACTION_KEY);
                }
            }
            set
            {
                if (IsInWebContext())
                {
                    HttpContext.Current.Items[TRANSACTION_KEY] = value;
                }
                else
                {
                    CallContext.SetData(TRANSACTION_KEY, value);
                }
            }
        }

        /// <summary>
        /// If within a web context, this uses <see cref="HttpContext" /> instead of the WinForms 
        /// specific <see cref="CallContext" />.  Discussion concerning this found at 
        /// http://forum.springframework.net/showthread.php?t=572.
        /// </summary>
        private ISession Session
        {
            get
            {
                if (IsInWebContext())
                    return (ISession)HttpContext.Current.Items[SESSION_KEY];
                else
                    return (ISession)CallContext.GetData(SESSION_KEY);
            }
            set
            {
                if (IsInWebContext())
                    HttpContext.Current.Items[SESSION_KEY] = value;
                else
                    CallContext.SetData(SESSION_KEY, value);
            }
        }

        /// <summary>
        /// Verifica se está no contexto Web.
        /// </summary>
        /// <returns></returns>
        private bool IsInWebContext()
        {
            var result = HttpContext.Current != null;
            return result;
        }

        /// <summary>
        /// Inicializa a Session Factory do nHibernate.
        /// </summary>
        public void InitializeSessionFactory()
        {
            if (_sessionFactory == null)
            {
                var configuration = new Configuration();
                try
                {
                    configuration.Configure();
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Finaliza a SessionFactory criada.
        /// </summary>
        public void CloseSessionFactory()
        {
            GetSessionFactory().Close();
        }

        /// <summary>
        /// Cria o Schema do Banco a partir das classes mapeadas *.hbm.xml
        /// </summary>
        public void ExportSchema()
        {
            //cria uma nova cgf no NHibernate
            var cfg = new Configuration();
            cfg.Configure();

            //Executando o schema export
            var schemaExport = new SchemaExport(cfg);
            schemaExport.Execute(true, true, false);
        }

        /// <summary>
        /// Cria o Schema do Banco a partir das classes mapeadas *.hbm.xml
        /// </summary>
        /// <param name="fileName">hibernate.hbm.xml</param>
        public void ExportSchema(String fileName)
        {
            //cria uma nova cgf no NHibernate
            var cfg = new Configuration();
            cfg.Configure(fileName);

            //Executando o schema export
            var schemaExport = new SchemaExport(cfg);
            schemaExport.Execute(true, true, false);
        }
    }
}
