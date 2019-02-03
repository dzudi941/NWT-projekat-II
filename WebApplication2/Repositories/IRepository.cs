using System;
using System.Collections.Generic;

namespace WebApplication2.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        IEnumerable<T> GetWhere(Func<T, bool> predicate);
       
        IEnumerable<T> FindByIds(IEnumerable<int> ids);
        void Insert(T obj);
        T Delete(int id);
        void Save();
    }
}
