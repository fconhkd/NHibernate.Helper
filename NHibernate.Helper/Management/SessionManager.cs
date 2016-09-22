using System;
using System.Web;
using System.Runtime.Remoting.Messaging;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace NHibernate.Helper.Management
{
    [Serializable]
    public class SessionManager
    {
        [ThreadStatic]
        //private static ISession _session; //this session is not used in web
        private static ISessionFactory _sessionFactory = null;
        //private static ISession _session = null;
        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTION";
        private const string SESSION_KEY = "CONTEXT_SESSION";

        /// <summary>
        /// Retorna a SessionFactory do NHibernate
        /// </summary>
        /// <returns>ISessionFactory</returns>
        public static ISessionFactory GetSessionFactory()
        {
            InitializeSessionFactory();
            return _sessionFactory;
        }

        /// <summary>
        /// Retorna uma nova Session para ser usado na aplicação
        /// </summary>
        /// <returns>ISession</returns>
        protected static ISession GetSession()
        {
            return GetSessionFactory().OpenSession();
        }

        /// <summary>
        /// Cria e inicializa a sessão para ser usada no contexto.
        /// </summary>
        public static void OpenSession()
        {
            //Seta uma sessão nova para ser usada.
            Session = GetSession();
        }

        /// <summary>
        /// Session de contexto; assume que a mesma existe.
        /// </summary>
        public static ISession Session
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
        private static bool IsInWebContext()
        {
            return HttpContext.Current != null;
        }

        /// <summary>
        /// Inicializa a Session Factory do nHibernate.
        /// </summary>
        public static void InitializeSessionFactory()
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
        public static void CloseSessionFactory()
        {
            GetSessionFactory().Close();
        }

        /// <summary>
        /// Cria o Schema do Banco a partir das classes mapeadas *.hbm.xml
        /// </summary>
        public static void ExportSchema()
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
        public static void ExportSchema(String fileName)
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
