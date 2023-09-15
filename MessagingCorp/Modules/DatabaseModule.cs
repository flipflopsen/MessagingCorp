using MessagingCorp.Services.API;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Modules
{
    public class DatabaseModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IDatabaseAccess>().To<IDatabaseAccess>();
        }
    }
}
