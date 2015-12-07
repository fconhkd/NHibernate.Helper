using System;
using System.Collections.Generic;

namespace NHibernate.Helper.Generics
{
    [Serializable]
    [Aspect.TransactionManagementAspect]
    public abstract class GenericBusiness<TID, T, TDao> : IDisposable
        where T : GenericEntity<TID>
        where TDao : GenericDAO<TID, T>, new()
    {
        #region Fields
        private TDao _dao;
        #endregion

        public TDao Dao
        {
            get
            {
                if (_dao == null)
                    _dao = new TDao();
                return _dao;
            }
        }

        public virtual void Save(T obj)
        {
            try
            {
                Dao.Save(obj);
            }
            catch (System.Exception e)
            {
                //TODO Impementar camada de log
                throw e;
            }
        }

        public virtual void Save(List<T> listObj)
        {
            try
            {
                Dao.Save(listObj);
            }
            catch (System.Exception e)
            {
                //TODO Impementar camada de log
                throw e;
            }
        }

        public virtual void Delete(TID id)
        {
            try
            {
                Dao.Delete(id);
            }
            catch (System.Exception e)
            {
                //TODO Impementar camada de log
                throw e;
            }
        }

        public virtual void Delete(T obj)
        {
            try
            {
                Dao.Delete(obj);
            }
            catch (System.Exception e)
            {
                //TODO Impementar camada de log
                throw e;
            }
        }

        public virtual void Delete(List<T> listObj)
        {
            try
            {
                foreach (T item in listObj)
                {
                    Delete(item);
                }
            }
            catch (System.Exception e)
            {
                //TODO Impementar camada de log
                throw e;
            }
        }

        public virtual void Update(T obj)
        {
            try
            {
                Dao.Update(obj);
            }
            catch (System.Exception e)
            {
                //TODO Impementar camada de log
                throw e;
            }
        }

        public virtual T GetById(TID id)
        {
            try
            {
                return Dao.GetById(id);
            }
            catch (System.Exception e)
            {
                //TODO Impementar camada de log
                throw e;
            }
        }

        public virtual List<T> GetAll()
        {
            try
            {
                return (List<T>)Dao.GetAll();
            }
            catch (System.Exception e)
            {
                //TODO Impementar camada de log
                throw e;
            }
        }

        public virtual T GetByQuery(String query)
        {
            try
            {
                return Dao.GetByQuery(query);
            }
            catch (System.Exception)
            {
                //TODO Impementar camada de log
                throw;
            }
        }

        public virtual List<T> ListByQuery(String query)
        {
            try
            {
                return Dao.ListByQuery(query);
            }
            catch (System.Exception)
            {
                //TODO Impementar camada de log
                throw;
            }
        }

        public virtual int Count()
        {
            return Dao.Count();
        }

        public void Dispose()
        {
            //if (_dao != null)
            //{
            //    _dao.Dispose();
            //    _dao = null;
            //}
        }
    }
}
