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

        public IEnumerable<File> Search(string search)
        {
            return Connection.Get().Search(search);
        }

        public void Save(File file)
        {
            Connection.Get().Save(file);
        }
    }
}
