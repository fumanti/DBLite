using Microsoft.Data.Sqlite;
using System;
using System.Configuration;
using System.Linq;

namespace SFM.DBLite
{
    public static class Utilities
    {
        public static StringComparison CaseInsensitive = StringComparison.CurrentCultureIgnoreCase;

        public static string DBPath { get; set; } = ConfigurationManager.ConnectionStrings["DBLitePath"].ConnectionString;

        public static string GetTablePrimaryKey<T>()
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            bool findPk = properties.Any(p =>
                Attribute.IsDefined(p, typeof(PrimaryKeyAttribute))
                || p.Name.Equals("ID", CaseInsensitive)
                || p.Name.StartsWith("Id", CaseInsensitive) || p.Name.EndsWith("Id", CaseInsensitive)
            );

            if (!findPk)
            {
                throw new SqliteException($"Impossibile trovare una chiave primaria definita per la tabella {type.Name}", 0);
            }

            string pkColumnName = properties.FirstOrDefault(p =>
                Attribute.IsDefined(p, typeof(PrimaryKeyAttribute))
                || p.Name.Equals("ID", CaseInsensitive)
                || p.Name.StartsWith("Id", CaseInsensitive) || p.Name.EndsWith("Id", CaseInsensitive)
            ).Name;

            return pkColumnName;
        }
    }
}
