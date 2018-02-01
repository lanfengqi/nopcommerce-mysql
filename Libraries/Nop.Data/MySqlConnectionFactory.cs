using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure;

namespace Nop.Data {
    public class MySqlConnectionFactory : IDbConnectionFactory {


        private readonly string _baseConnectionString;
        private Func<string, DbProviderFactory> _providerFactoryCreator;

        public MySqlConnectionFactory() {
        }

        public MySqlConnectionFactory(string baseConnectionString) {
            this._baseConnectionString = baseConnectionString;
        }

        public DbConnection CreateConnection(string nameOrConnectionString) {
            string connectionString = nameOrConnectionString;

            bool treatAsConnectionString = nameOrConnectionString.IndexOf('=') >= 0;

            if (!treatAsConnectionString) {
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(this.BaseConnectionString);
                builder.Server = nameOrConnectionString;
                connectionString = builder.ConnectionString;
            }
            DbConnection connection = null;
            try {
                connection = this.ProviderFactory("MySql.Data.MySqlClient").CreateConnection();
                connection.ConnectionString = connectionString;
            } catch {
                connection = new MySqlConnection(connectionString);
            }
            return connection;
        }

        public string BaseConnectionString {
            get {
                return this._baseConnectionString;
            }
        }

        internal Func<string, DbProviderFactory> ProviderFactory {
            get {
                Func<string, DbProviderFactory> func1 = this._providerFactoryCreator;
                return delegate (string name) {
                    return DbProviderFactories.GetFactory(name);
                };
            }
            set {
                this._providerFactoryCreator = value;
            }
        }
    }
}
