using CommonTypes;
using DataAccess.DatabaseContext;
using Npgsql;
using System;
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

        /// <summary>
        /// Gibt an, ob eine Datenbankverbindung besteht
        /// </summary>
        public bool IsConnected { get; set; } = false;

        /// <summary>
        /// Stellt die Verbindung mit der Datenbank her
        /// </summary>
        public void Connect()
        {
            if (!IsConnected)
            {
                _Connection = new NpgsqlConnection();
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

        /// <summary>
        /// Trennt die Verbindung zur Datenbank
        /// </summary>
        public void Disconnect()
        {
            if (IsConnected)
            {
                _Connection.Close();
                _Connection.Dispose();
                IsConnected = false;
            }
        }

        /// <summary>
        /// Gibt die unterliegende Verbindung zurück.
        /// Wird für FileContext benötigt (s. FileContext und ContextBase Konstruktor)
        /// </summary>
        internal NpgsqlConnection GetConnection()
        {
            return _Connection;
        }

        /// <summary>
        /// Volltextsuche in der Datenbank, vergleich auf Content 
        /// </summary>
        /// <param name="filter">Der Text, nachdem die Datenbank durchsucht wird</param>
        /// <returns>Eine Auflistung von Dateien, welche den gesuchten Text enthalten</returns>
        public IEnumerable<File> Search(string filter)
        {
            IEnumerable<File> result = null;
            using (FileContext context = new FileContext(Get(), false))
            {
                result = context.Files.Where(x => NpgsqlTextFunctions.Match(NpgsqlTextFunctions.ToTsVector(x.Content), NpgsqlTextFunctions.PlainToTsQuery(filter))).ToList();
            }
            return result;
        }

        /// <summary>
        /// Speichert ein File in der Datenbank
        /// </summary>
        /// <param name="file">Das File, welches gespeichert wird</param>
        public void Save(File file)
        {
            using (FileContext context = new FileContext(Get(), false))
            {
                File existing = null;
                var files = context.Files.Where(x =>
                        x.Modified.Equals(file.Modified) &&
                        x.Created.Equals(file.Created) &&
                        x.Content.Equals(file.Content) &&
                        x.Name.Equals(file.Name) &&
                        x.Path.Equals(file.Path) &&
                        x.Type.Equals(file.Type)).ToList();

                if (files.Count > 0)
                {
                    existing = files[0];

                    context.Entry(existing).State = EntityState.Modified;
                    context.Entry(existing).Entity.Created = file.Created;
                    context.Entry(existing).Entity.Modified = file.Modified;
                    context.Entry(existing).Entity.Content = file.Content;
                    context.Entry(existing).Entity.Name = file.Name;
                    context.Entry(existing).Entity.Path = file.Path;
                    context.Entry(existing).Entity.Type = file.Type;
                }
                else
                {
                    context.Files.Add(file);
                }

                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("SaveChanges Error: " + e.Message);
                }
            }
        }
    }
}
