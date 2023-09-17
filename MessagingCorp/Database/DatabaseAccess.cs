using Ninject;
using SurrealDB.Configuration;
using SurrealDB.Driver.Rpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SurrealDB.Driver.Rest;

namespace MessagingCorp.Database
{
    public class DatabaseAccess
    {
        [Inject]
        private Config _cfg;

        private DatabaseRpc _dbRpc;

        public DatabaseAccess(Config config)
        {
            _cfg = config;
            _dbRpc = new DatabaseRpc(_cfg);
        }

        public async Task Connect()
        {
            if (_dbRpc == null)
                return;
            await _dbRpc.Open();
        }


    }
}
