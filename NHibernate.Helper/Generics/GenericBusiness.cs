using System;
using System.Collections.Generic;
using NHibernate.Helper.Management;

namespace NHibernate.Helper.Generics
{
    [Serializable]
    [Aspect.SessionManagementAspect(AspectPriority = 0, AttributeInheritance = PostSharp.Extensibility.MulticastInheritance.Strict)]
    public abstract class GenericBusiness<TID, T, TDao> : IDisposable
        where T : GenericEntity<TID>
        where TDao : GenericDAO<TID, T>, new()
    {
        #region Fields
        private TDao _dao;
        #endregion

        public GenericBusiness()
        {
            //Management.SessionManager.GetCurrentSession();
            Management.SessionManager.Instance.GetSession();
        }

        protected internal TDao Dao
        {
            get
            {
                if (_dao == null) _dao = new TDao();
                return _dao;
            }
        }

        [Aspect.TransactionManagementAspect(AspectPriority = 1)]
        public virtual void Save(T obj)
        {
            try
            {
                Dao.Save(obj);
            }
            catch (System.Exception)
            {
                //TODO Impementar camada de log
                throw;
            }
        }

        [Aspect.TransactionManagementAspect(AspectPriority = 1)]
        public virtual void Save(IList<T> listObj)
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

        [Aspect.TransactionManagementAspect(AspectPriority = 1)]
        public virtual void SaveOrUpdate(T obj)
        {
            try
            {
                Dao.SaveOrUpdate(obj);
            }
            catch (System.Exception ex)
            {
                //TODO Impementar camada de log
                throw ex;
            }
        }

        [Aspect.TransactionManagementAspect(AspectPriority = 1)]
        public virtual void SaveOrUpdate(IList<T> list)
        {
            try
            {
                Dao.SaveOrUpdate(list);
            }
            catch (System.Exception)
            {
                //TODO Impementar camada de log
                throw;
            }
        }

        public virtual T SaveAndReturn(T obj)
        {
            Save(obj);
            return obj;
        }

        [Aspect.TransactionManagementAspect(AspectPriority = 1)]
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

        [Aspect.TransactionManagementAspect(AspectPriority = 1)]
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

        [Aspect.TransactionManagementAspect(AspectPriority = 1)]
        public virtual int Delete(String query)
        {
            try
            {
                return Dao.Delete(query);
            }
            catch (System.Exception e)
            {
                //TODO Impementar camada de log
                throw e;
            }
        }

        [Aspect.TransactionManagementAspect(AspectPriority = 1)]
        public virtual void Delete(List<T> listObj)
        {
            try
            {
                foreach (T item in listObj)
                {
                    Dao.Delete(item);
                }
            }
            catch (System.Exception e)
            {
                //TODO Impementar camada de log
                throw e;
            }
        }

        [Aspect.TransactionManagementAspect(AspectPriority = 1)]
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

        public virtual void Evict(T o)
        {
            Dao.Evict(o);
        }

        public virtual void BeginTransaction()
        {
            //begin trans   
            //var session = SessionManager.Session;
            //if (!session.Transaction.IsActive)
            //    session.BeginTransaction();
            Management.SessionManager.Instance.BeginTransaction();
        }

        public virtual void CommitTransaction()
        {
            //var session = SessionManager.Session;
            //if (session.Transaction != null &&
            //            session.Transaction.IsActive)
            //{
            //    session.Flush(); session.Transaction.Commit();
            //}
            Management.SessionManager.Instance.CommitTransaction();
        }

        public virtual void RollbackTransaction()
        {
            Management.SessionManager.Instance.RollbackTransaction();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    //var session = Management.SessionManager.Session;
                    //session.Close();
                    //Management.SessionManager.Session = null;
                    Management.SessionManager.Instance.CloseSession();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~GenericBusiness() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
