using MessagingCorp.Common.Logger;
using MessagingCorp.EntityManagement.API;
using MessagingCorp.Services.API;
using Serilog;
using Serilog.Events;

namespace MessagingCorp.Services.Core
{
    public class MessageFacService : IMessageFacService
    {
        public required IUserManagement UserManagment { protected get; init; }

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
