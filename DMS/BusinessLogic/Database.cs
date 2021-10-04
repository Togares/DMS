using CommonTypes;
using DataAccess;
using DataAccess.DatabaseContext;
using System.Collections.Generic;

namespace BusinessLogic
{
    public class Database
    {
        public Database()
        {
            Connection.Get().Connect();
        }

        public bool HasConnection => Connection.Get().IsConnected;

        public IEnumerable<File> Search(string search)
        {
            return Connection.Get().Search(search);
        }

        public void Save(File file)
        {
            try
            {
                Connection.Get().Save(file);
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }
    }
}
