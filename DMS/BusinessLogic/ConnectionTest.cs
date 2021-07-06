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
            string host = "schuldb1.its-stuttgart.de";
            int port = 5432;
            con.Connect(user, user, host, port, db);
        }
        
    }
}
