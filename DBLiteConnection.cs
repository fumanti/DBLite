using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SFM.DBLite
{
    public class DBLiteConnection : SqliteConnection
    {
        private const string DefaultPkName = "ID";

        public List<DBLiteTable> Tables { get; set; } = new List<DBLiteTable>();

        /// <summary>
        /// Recupera una tabella da DBLite, se non esiste la crea
        /// </summary>
        /// <param name="name">Nome della tabella</param>
        /// <returns>DBLiteTable</returns>
        public DBLiteTable<T> GetTable<T>()
        {
            string name = typeof(T).Name;
            var trial = 0;
            var table = this.Tables.FirstOrDefault(t => t.TableName.Equals(name, StringComparison.CurrentCultureIgnoreCase)) as DBLiteTable<T>;
            if (table == null && ++trial < 10)
            {
                this.AddTable<T>(name);
                table = this.GetTable<T>();
            }
            return table;
        }


        public DBLiteConnection() : base()
        {

        }

        public DBLiteConnection(string dbPath) : base(dbPath)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_winsqlite3());
            this.Open();
        }

        private void AddTable<T>(string name)
        {
            var instance = (T)Activator.CreateInstance(typeof(T));
            var table = DBLiteTable.From(instance);
            try
            {
                var command = this.CreateTable(table);
                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
            }
            this.Tables.Add(table);
        }

        internal int Delete<T>(T model)
        {
            DBLiteTable<T> table = this.GetTable<T>();
            var command = this.Delete(table, model);
            return command.ExecuteNonQuery();
        }

        internal int Update<T>(T model)
        {
            DBLiteTable<T> table = this.GetTable<T>();
            var command = this.Update(table, model);
            return command.ExecuteNonQuery();
        }

        public int Insert<T>(T model)
        {
            DBLiteTable<T> table = this.GetTable<T>();
            var command = this.Insert(table, model);
            return command.ExecuteNonQuery();
        }


        public T Select<T>(int value, string columnName = null) where T : new()
        {
            return this.Select<T>(value.ToString(), columnName);
        }

        public T Select<T>(string value, string columnName = null) where T : new()
        {
            T result = new T();

            DBLiteTable<T> table = GetTable<T>();
            columnName = columnName ?? table.GetPrimaryKey() ?? DefaultPkName;
            var command = this.Select(table, new[] { new SqliteParameter(columnName, value) });

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    foreach (string key in result.GetKeysAsArray())
                    {
                        var val = reader[key].ToString();
                        result.SetValue(key, val);
                    }
                }
            }
            return result;
        }
    }
}
