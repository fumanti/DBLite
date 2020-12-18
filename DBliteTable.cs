using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SFM.DBLite
{
    public abstract class DBLiteTable
    {
        public string TableName { get; set; }

        public IEnumerable<DBliteColumn> Columns { get; set; }

        public DBliteColumn GetColumn(string name)
        {
            return this.Columns.FirstOrDefault(c => c.ColumnName == name);
        }

        public DBliteColumn GetColumn(int index)
        {
            return this.Columns.ToArray()[index];
        }

        public string GetPrimaryKey()
        {
            return this.Columns.FirstOrDefault(t => t.IsKey.GetValueOrDefault()).ColumnName;
        }

        public static DBLiteTable<T> From<T>(T model)
        {
            string name = model.GetType().Name;
            var columns = new DBliteColumnCollection<T>(model);
            return new DBLiteTable<T>(name, columns);
        }
    }

    public class DBLiteTable<T> : DBLiteTable
    {

        public DBLiteTable()
        {

        }

        public DBLiteTable(string name, IEnumerable<DBliteColumn> columns)
        {
            this.TableName = name;
            this.Columns = columns;
        }

        //public static DBLiteTable<T> From(T model)
        //{
        //    string name = model.GetType().Name;
        //    var columns = new DBliteColumnCollection<T>(model);
        //    return new DBLiteTable<T>(name, columns);
        //}

        //public DBliteColumn GetColumn(string name)
        //{
        //    return this.Columns.FirstOrDefault(c => c.ColumnName == name);
        //}


    }
}
