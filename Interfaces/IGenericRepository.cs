using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFM.DBLite.Interfaces
{
    public interface IGenericRepository
    {
        int Insert<T>(T model);
        int Update<T>(T model);
        bool Delete<T>(T model);
        T Select<T>(int pkValue, string pkName = null) where T : new();
        //T[] SelectAll<T>() where T : new();
    }
}
