using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjektB.Web.Infrastructure
{
    public class UnitOfWork : IDisposable
    {
        private Dictionary<string, Object> _collection;

        public UnitOfWork()
        {
            _collection = new Dictionary<string, object>();
        }

        public T Get<T>(string key) where T: class
        {
            object obj;
            if (!_collection.TryGetValue(key, out obj))
            {
                return null;
            }
            return obj as T;
        }

        public object this[string key]
        {
            get 
            {
                object obj;
                if (!_collection.TryGetValue(key, out obj))
                {
                    return null;
                }
                return obj; 
            }
            set { _collection[key] = value; }
        }

        //TODO: think about making it async
        public void Commit()
        {
            foreach (var obj in _collection.Values)
            {
                if (obj is DbContext)
                {
                    ((DbContext)obj).SaveChanges();
                }
            }

            foreach (var obj in _collection.Values)
            {
                if (obj is DbContextTransaction)
                {
                    ((DbContextTransaction)obj).Commit();
                }
            }
        }

        public void Rollback()
        {
            foreach (var obj in _collection.Values)
            {
                if (obj is DbContextTransaction)
                {
                    var tran = (DbContextTransaction)obj;
                    tran.Rollback();
                }
            }
        }

        public void Dispose()
        {
            Commit();

            foreach (var obj in _collection.Values)
            {
                if (obj is IDisposable)
                {
                    ((IDisposable)obj).Dispose();
                }
            }
        }
    }
}