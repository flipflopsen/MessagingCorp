using MessagingCorp.Services.Core;

namespace MessagingCorp.ApplicationStartup
{
    public static class NinjectStartup
    {
        public static async Task Startup()
        {
            var service = new MessageCorpService();

            service.InitializeService();
            await service.StartOperation();

            _ = Console.ReadKey();
        }
    }
}
