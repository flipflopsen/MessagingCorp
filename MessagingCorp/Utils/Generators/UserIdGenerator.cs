using Castle.Core.Internal;
using MessagingCorp.Common.Generators.Exceptions;
using MessagingCorp.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Utils
{
    public static class UserIdGenerator
    {
        private const string PREFIX = "GrownCorp-";

        public static string GenerateNewUserUid()
        {
            return PREFIX + Guid.NewGuid().ToString("N").Substring(0, 12).Replace("-", "");
        }
    }
}
