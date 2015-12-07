using System;
using System.Collections.Generic;
using NHibernate;

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
                return Management.SessionManager.Session;
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
                Save(item);
            }
        }

        protected internal void SaveOrUpdate(T o)
        {
            Session.SaveOrUpdate(o);
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
        /// Atualizar objeto persistido
        /// </summary>
        /// <param name="o"></param>
        protected internal void Update(T o)
        {
            Session.Update(o);
        }

        /// <summary>
        /// Recupera um ID especifico
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        protected internal T GetById(TID id)
        {
            return Session.Load<T>(id);
        }


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
        /// Commit das alterações se necessario
        /// </summary>
        protected internal void CommitChanges()
        {

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
        /// Counts the number of records.
        /// </summary>
        protected internal int Count()
        {
            ICriteria criteria = CreateCriteria();
            return criteria.SetProjection(NHibernate.Criterion.Projections.RowCount()).UniqueResult<int>();
        }

        protected internal System.Type persitentType = typeof(T);

        /// <summary>
        /// Destruidor da classe
        /// </summary>
        public void Dispose() { }
    }
}
