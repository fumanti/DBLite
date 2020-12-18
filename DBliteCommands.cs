using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace SFM.DBLite
{
    public static class DBliteCommands
    {
        public static SqliteCommand Insert<T>(this DBLiteConnection connection, DBLiteTable table, T model)
        {
            object[] parameters = model.GetValuesAsArray();
            var sb = new StringBuilder($"INSERT INTO {table.TableName} VALUES (");
            for (int n = 0; n < table.Columns.Count(); n++)
            {
                if (n > 0)
                {
                    sb.Append(", ");
                }
                sb.Append($"\"{parameters[n]}\"");
            }
            sb.Append(");");
            var sql = sb.ToString();
            var command = new SqliteCommand(sql, connection);
            return command;
        }

        public static SqliteCommand Select<T>(this DBLiteConnection connection, DBLiteTable<T> table, IEnumerable<DbParameter> parameters)
        {
            var sb = new StringBuilder($"SELECT {string.Join(", ", table.Columns.Select(c => c.ColumnName))} FROM {table.TableName}");
            sb.Where(parameters);
            sb.Append(";");
            var sql = sb.ToString();
            var command = new SqliteCommand(sql, connection);
            return command;
        }

        public static SqliteCommand Update<T>(this DBLiteConnection connection, DBLiteTable<T> table, T model)
        {
            var keys = model.GetKeysAsArray();
            var sb = new StringBuilder($"UPDATE {table.TableName} SET ");
            for (int n = 0; n < table.Columns.Count(c => !c.IsKey.GetValueOrDefault()); n++)
            {
                var modelPropertyKey = keys.FirstOrDefault(c => c == table.GetColumn(n).ColumnName);
                var modelPropertyValue = model.GetValue<T>(modelPropertyKey);
                if (n > 0)
                {
                    sb.Append(", ");
                }
                sb.Append($"{modelPropertyKey} =\"{modelPropertyValue}\"");
            }
            // WHERE
            string key = table.GetPrimaryKey();
            var value = model.GetValue(key);
            sb.Where(new SqliteParameter[] { new SqliteParameter(key, value) });
            sb.Append(";");
            var sql = sb.ToString();
            var command = new SqliteCommand(sql, connection);
            return command;
        }

        public static SqliteCommand Delete<T>(this DBLiteConnection connection, DBLiteTable<T> table, T model)
        {
            var sb = new StringBuilder($"DELETE FROM {table.TableName}");
            // WHERE
            string key = table.GetPrimaryKey();
            var value = model.GetValue(key);
            sb.Where(new[] { new SqliteParameter(key, value) });
            sb.Append(";");
            var sql = sb.ToString();
            var command = new SqliteCommand(sql, connection);
            return command;
        }

        private static void Where(this StringBuilder sb, IEnumerable<DbParameter> parameters)
        {
            for (int n = 0; n < parameters.Count(); n++)
            {
                var parameter = parameters.ElementAt(n);
                if (n == 0)
                {
                    sb.Append(" WHERE ");
                }
                else
                {
                    sb.Append(" AND ");
                }
                sb.Append($"{parameter.ParameterName} = \"{parameter.Value}\"");
            }
        }

        public static SqliteCommand CreateTable(this DBLiteConnection connection, DBLiteTable table)
        {
            StringBuilder sb = new StringBuilder($"CREATE TABLE {table.TableName} (");
            for (int n = 0; n < table.Columns.Count(); n++)
            {
                var col = table.Columns.ElementAt(n);
                if (n > 0)
                {
                    sb.Append(", ");
                }
                sb.Append($"{col.ColumnName} {col.DataTypeName}");
                sb.Append(col.ColumnSize != null ? $"({col.ColumnSize})" : null);
                sb.Append(col.IsKey.GetValueOrDefault() ? " PRIMARY KEY " : null);
            }
            sb.Append(");");
            string sql = sb.ToString();

            var command = new SqliteCommand(sql, connection);
            return command;
        }


    }
}
