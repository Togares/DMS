using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class ConnectionTest
    {
        public ConnectionTest()
        {
            Connection con = new Connection();
            string user = "u_e2fi5githubprofis";
            string db = "E2FI5GitHubProfis"; // DMS
            string host = "schuldb2.its-stuttgart.de"; // 10.194.9.131
            int port = 5432;
            con.Connect(user, user, host, port, db);
        }
        
    }
}
