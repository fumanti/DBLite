using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace SFM.DBLite
{
    public class DBliteColumn : DbColumn
    {
        public DBliteColumn(string name, SqlDbType type, bool primaryKey = false,  int? size = null)
        {
            this.ColumnName = name;
            this.DataTypeName = type.ToString();
            this.ColumnSize = size;
            this.IsKey = primaryKey;
        }
    }

    public class DBliteColumnCollection<T> : Collection<DBliteColumn>
    {
        public DBliteColumnCollection(T model)
        {
            var keys = model.GetKeysAsArray();
            for (int n = 0; n < keys.Length; n++)
            {
                PropertyInfo pInfo = model.GetType().GetProperty(keys[n].ToString());
                string name = pInfo.Name;
                var dbType = pInfo.PropertyType == typeof(int) ? SqlDbType.Int : SqlDbType.NVarChar;

                bool isPrimary = Utilities.GetTablePrimaryKey<T>().Equals(name);

                var column = new DBliteColumn(name, dbType, isPrimary);
                Add(column);
            }
        }
    }
}
