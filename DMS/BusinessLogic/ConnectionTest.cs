using DataAccess;

namespace BusinessLogic
{
    public class ConnectionTest
    {
        public ConnectionTest()
        {            
            Connection.Get().Connect();
        }
    }
}
