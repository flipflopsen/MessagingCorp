using MessagingCorp.Providers;
using MessagingCorp.Providers.API;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Modules
{
    public class CryptoModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICryptoProvider>().To<CryptoProvider>();
            this.Bind<IKeyProvider>().To<KeyProvider>();
        }
    }
}
