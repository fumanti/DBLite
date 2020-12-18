using Microsoft.Data.Sqlite;
using SFM.DBLite.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFM.DBLite
{
    public class GenericRepository : IGenericRepository
    {   
        #region Context Property
        DBLiteConnection _context;

        protected DBLiteConnection Context
        {
            get
            {
                return _context;
            }
            set
            {
                _context = value;
            }
        }
        #endregion

        #region Constructor
        public GenericRepository()
        {
            Context = new DBLiteConnection(Utilities.DBPath);
        }
        #endregion

        #region Generic Repository
        public int Insert<T>(T model)
        {
            try
            {
                int result = Context.Insert(model);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int Update<T>(T model)
        {
            int result = Context.Update(model);
            return result;
        }

        public bool Delete<T>(T model)
        {
            int result = Context.Delete(model);
            return result.Equals(1);
        }

        public T Select<T>(int value, string columnName = null) where T : new()
        {
            T result = Context.Select<T>(value, columnName);
            return result;
        }

        //public T[] SelectAll<T>() where T : new()
        //{
        //    throw new NotImplementedException();
        //}

        #endregion

        #region IDispose Region
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
