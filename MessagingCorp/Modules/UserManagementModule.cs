using MessagingCorp.EntityManagement;
using MessagingCorp.EntityManagement.API;
using Ninject.Modules;

namespace MessagingCorp.Modules
{
    public class UserManagementModule : NinjectModule
    {
        // needs database and maybe encryption
        public override void Load()
        {
            Bind<IUserManagement>().To<UserManagement>().InSingletonScope();
        }
    }

}
