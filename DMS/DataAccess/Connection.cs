using Npgsql;

namespace DataAccess
{
    public class Connection
    {
        private NpgsqlConnection _Connection;

        public Connection()
        {
            _Connection = new NpgsqlConnection();
        }

        public void Connect(string user, string password, string host, int port, string database)
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            builder.Username = user;
            builder.Password = password;
            builder.Database = database;
            builder.Port = port;
            builder.Host = host;
            _Connection.ConnectionString = builder.ConnectionString;
            _Connection.Open();
        }

        public void Disconnect()
        {
            _Connection.Close();
        }
    }
}
