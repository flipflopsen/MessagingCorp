﻿using MessagingCorp.ApplicationStartup;

namespace MessagingCorp
{
    public static class Startup
    {
        public static async Task Main(string[] args)
        {
            await NinjectStartup.Startup();
            //AutofacStartup.Startup();
        }
    }
}
