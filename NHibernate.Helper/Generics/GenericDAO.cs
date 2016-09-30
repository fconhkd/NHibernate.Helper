using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;

namespace NHibernate.Helper.Generics
{
    [Serializable]
    public abstract class GenericDAO<TID, T> : IDisposable
        where T : GenericEntity<TID>
    {

        //private ISession _session;
        private ISession Session
        {
            get
            {
                return Management.SessionManager.Instance.GetSession();
                //return Management.SessionManager.GetCurrentSession();
            }
        }

        /// <summary>
        /// Salvar objeto genérico
        /// </summary>
        /// <param name="o"></param>
        protected internal void Save(T o)
        {
            Session.Save(o);
        }

        /// <summary>
        /// Salva lista generica
        /// </summary>
        /// <param name="o"></param>
        protected internal void Save(IList<T> o)
        {
            foreach (T item in o)
            {
                try
                {
                    Save(item);
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Salva ou atualiza
        /// </summary>
        /// <param name="o">Object do Tipo T</param>
        protected internal void SaveOrUpdate(T o)
        {
            Session.SaveOrUpdate(o);
        }

        /// <summary>
        /// Salva ou atualiza uma lista
        /// </summary>
        /// <param name="o">Object do Tipo T</param>
        protected internal void SaveOrUpdate(IList<T> list)
        {
            foreach (T item in list)
            {
                Session.SaveOrUpdate(item);
            }
        }

        /// <summary>
        /// Deleta objeto generico
        /// </summary>
        /// <param name="o"></param>
        protected internal void Delete(T o)
        {
            Session.Delete(o);
        }

        /// <summary>
        /// Deleta objeto a partir do seu id
        /// </summary>
        /// <param name="id"></param>
        protected internal void Delete(TID id)
        {
            Delete(GetById(id));
        }

        /// <summary>
        /// Deleta todos objetos retornados pela query
        /// </summary>
        /// <param name="query">query para pesquisa</param>
        /// <returns>quantidados de registros</returns>
        protected internal int Delete(string query)
        {
            return Session.Delete(query);
        }

        /// <summary>
        /// Atualizar objeto persistido
        /// </summary>
        /// <param name="o"></param>
        protected internal void Update(T o)
        {
            Session.Update(o);
        }

        /// <summary>
        /// Atualizar uma lista de objetos
        /// </summary>
        /// <param name="o"></param>
        protected internal void Update(IList<T> list)
        {
            foreach (var item in list)
            {
                try
                {
                    Session.Update(item);
                }
                catch (System.Exception)
                {

                    throw;
                }
            }
        }

        /// <summary>
        /// Recupera um ID especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal T GetById(TID id)
        {
            return Session.Get<T>(id);
        }

        /// <summary>
        /// Recupera um registro mesmo em lock
        /// </summary>
        /// <param name="id">identificador</param>
        /// <param name="shoudLock">LockMode Upgrade</param>
        /// <returns>Object do Tipo T</returns>
        protected internal T GetById(TID id, bool shoudLock)
        {
            T entity;

            if (shoudLock)
            {
                entity = Session.Load<T>(id, LockMode.Upgrade);
            }
            else
            {
                entity = Session.Load<T>(id);
            }
            return entity;
        }

        /// <summary>
        /// Metodo Generico que lista todos os que encontrar
        /// </summary>
        /// <returns></returns>
        protected internal IList<T> GetAll()
        {
            ICriteria criteria = Session.CreateCriteria(typeof(T));
            return criteria.List<T>();
        }

        /// <summary>
        /// Metodo Generico, Recupera um unico objeto por Query
        /// </summary>
        /// <param name="query"></param>
        /// <returns>T</returns>
        protected internal T GetByQuery(String query)
        {
            T o = Session.CreateQuery(query).UniqueResult<T>();
            return o;
        }

        /// <summary>
        /// Metodo Generico lista por query
        /// </summary>
        /// <param name="query"></param>
        /// <returns>List<T></returns>
        protected internal List<T> ListByQuery(String query)
        {
            IQuery myQuery = Session.CreateQuery(query);
            List<T> list = (List<T>)myQuery.List<T>();
            return list;
        }

        /// <summary>
        /// Realiza a consulta utilizando um objeto de exemplo, funciona sempre no nivel do objeto pai
        /// </summary>
        /// <param name="obj">Objeto utilizado de exemplo</param>
        /// <param name="mode">Tipo de like utilizado na consulta</param>
        /// <returns>Lista de objetos</returns>
        protected internal IList<T> QueryByExample(T obj, MatchMode mode)
        {
            Example example = Example.Create(obj).EnableLike(mode);
            return Session.CreateCriteria(typeof(T)).Add(example).List<T>();
        }

        protected internal Example CreateExample(object entity)
        {
            return Example.Create(entity);
        }

        /// <summary>
        /// Retorna uma criteria do tipo
        /// </summary>
        /// <returns></returns>
        protected internal ICriteria CreateCriteria()
        {
            return Session.CreateCriteria(persitentType);
        }

        /// <summary>
        /// Retorna uma Query do Hibernate
        /// </summary>
        /// <param name="queryString">string hql</param>
        /// <returns>IQuery</returns>
        protected IQuery CreateQuery(String queryString)
        {
            return Session.CreateQuery(queryString);
        }

        protected ISQLQuery CreateSqlQuery(String queryString)
        {
            return Session.CreateSQLQuery(queryString);
        }

        protected IMultiQuery CreateMultiQuery()
        {
            return Session.CreateMultiQuery();
        }

        protected IMultiCriteria CreateMultiCriteria()
        {
            return Session.CreateMultiCriteria();
        }

        /// <summary>
        /// Counts the number of records.
        /// </summary>
        protected internal int Count()
        {
            ICriteria criteria = CreateCriteria();
            return criteria.SetProjection(NHibernate.Criterion.Projections.RowCount()).UniqueResult<int>();
        }

        /// <summary>
        /// Remove o objeto do cache
        /// </summary>
        /// <param name="o">obj a ser removido do cache</param>
        protected internal void Evict(T o)
        {
            Session.Evict(o);
        }

        protected internal System.Type persitentType = typeof(T);

        /// <summary>
        /// Destruidor da classe
        /// </summary>
        public void Dispose() { }
    }
}
