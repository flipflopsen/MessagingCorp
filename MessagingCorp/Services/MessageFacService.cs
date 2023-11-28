using MessagingCorp.EntityManagement.API;
using MessagingCorp.Services.API;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingCorp.Utils.Logger;

namespace MessagingCorp.Services
{
    public class MessageFacService : IMessageFacService
    {
        public required IUserManagement UserManagment {protected get; init; }

        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<MessageFacService>("./Logs/MessageFacService.log", true, LogEventLevel.Debug);


        public MessageFacService()
        {

        }

        public void Test(string msg)
        {
            var sth = UserManagment.GetUser("123");
            Console.WriteLine(msg);
        }
    }
}
