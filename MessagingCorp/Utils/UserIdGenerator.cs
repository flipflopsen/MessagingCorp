using Castle.Core.Internal;
using MessagingCorp.Exceptions;
using MessagingCorp.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Utils
{
    public class UserIdGenerator
    {
        public IDatabaseAccess _db;
        private const string PREFIX = "GrownCorp-";

        public string GenerateNewUserUid()
        {
            string uid = string.Empty;
            var isNew = false;

            while (!isNew)
            {
                uid = Guid.NewGuid().ToString("N").Substring(0, 12).Replace("-", "");
                if (_db.IsUidExistent(PREFIX + uid))
                    isNew = true;
            }

            if (uid.IsNullOrEmpty())
                throw new UidCreationException($"Weren't able to generate a new UID, figure this out ASAP!");
            return PREFIX + uid;
        }
    }
}
