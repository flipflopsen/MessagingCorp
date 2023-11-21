using MessagingCorp.Auth;
using MessagingCorp.EntityManagement;
using MessagingCorp.EntityManagement.API;
using MessagingCorp.Services.API;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Modules
{
    public class UserManagementModule : NinjectModule
    {
        // needs database and maybe encryption
        public override void Load()
        {
            Bind<IUserManagement>().To<UserManagement>();
        }
    }

}
