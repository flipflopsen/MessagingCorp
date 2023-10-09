using MessagingCorp.Providers.API;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Modules
{
    public class CachingModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Implement provider and good
            Bind<ICachingProvider>().To<ICachingProvider>();
        }
    }
}
