using CommonTypes;
using DataAccess.DatabaseContext;
using Npgsql;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class Connection
    {
        private const string user = "u_e2fi5githubprofis";
        private const string password = user;
        private const string db = "E2FI5GitHubProfis";
        private const string host = "schuldb1.its-stuttgart.de";
        private const int port = 5432;

        private NpgsqlConnection _Connection;

        #region Singleton

        private static Connection _Instance = null;

        private Connection()
        {
            _Connection = new NpgsqlConnection();
        }

        public static Connection Get()
        {
            return _Instance = _Instance == null ? new Connection() : _Instance;
        }

        #endregion Singleton

        #region Config

        private class NpgsqlConfiguration : DbConfiguration
        {
            public NpgsqlConfiguration() : base()
            {
                SetProviderFactory("Npgsql", NpgsqlFactory.Instance);

                SetProviderServices("Npgsql", NpgsqlServices.Instance);

                SetDefaultConnectionFactory(new NpgsqlConnectionFactory());
            }
        }

        public DbConfiguration Configuration { get; set; } = new NpgsqlConfiguration();

        #endregion Config

        public bool IsConnected { get; set; } = false;

        public void Connect()
        {
            if (!IsConnected)
            {
                NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
                builder.Username = user;
                builder.Password = password;
                builder.Database = db;
                builder.Port = port;
                builder.Host = host;
                _Connection.ConnectionString = builder.ConnectionString;
                try
                {
                    _Connection.Open();
                }
                catch (System.Net.Sockets.SocketException)
                {
                    Debug.WriteLine("SocketException upon connection to the database");
                    IsConnected = false;
                    return;
                }
                IsConnected = true;
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                _Connection.Close();
                IsConnected = false;
            }
        }

        internal NpgsqlConnection GetConnection()
        {
            return _Connection;
        }

        public IEnumerable<File> Search(string filter)
        {
            IEnumerable<File> result = null;
            using (FileContext context = new FileContext(Get(), false))
            {
                result = context.Files.Where(x => NpgsqlTextFunctions.Match(NpgsqlTextFunctions.ToTsVector(x.Content), NpgsqlTextFunctions.PlainToTsQuery(filter))).ToList();
            }
            return result;
        }

        public void Save(File file)
        {
            using (FileContext context = new FileContext(Get(), false))
            {
                context.Files.Add(file);
                context.SaveChanges();
            }
        }
    }
}
