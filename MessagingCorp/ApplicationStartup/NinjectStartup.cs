using MessagingCorp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.ApplicationStartup
{
    public static class NinjectStartup
    {
        public static async Task Startup()
        {
            var service = new MessageCorpService();
            await service.InitializeService();
            await service.StartOperation();

            var exit = Console.ReadKey();
        }
    }
}
