﻿using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using MessagingCorp.Services.Authentication;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Modules
{
    public class AuthenticationModule : NinjectModule
    {
        // needs database and maybe encryption
        public override void Load()
        {
            Bind<IAuthenticationGovernment>().To<BaseAuthenticationService>();
        }
    }
}
