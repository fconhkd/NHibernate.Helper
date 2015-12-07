using System;
using System.Collections.Generic;
using NHibernate;

namespace NHibernate.Helper.Generics
{
    public interface IDAO<TID, T>
    {
        T GetById(TID id);
        T GetById(TID id, bool shouldLock);
        IList<T> GetAll();
        List<T> ListByQuery(String query);
        void Save(T obj);
        void SaveOrUpdate(T obj);
        void Save(IList<T> obj);
        void Update(T obj);
        //void Update(IList<T> obj);
        void Delete(T obj);
        void CommitChanges();
        ICriteria CreateCriteria();
        int Count();
    }
}
