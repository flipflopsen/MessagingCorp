using MessagingCorp.Database;
using MessagingCorp.Services.API;
using Ninject.Modules;


namespace MessagingCorp.Modules
{
    public class DatabaseModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IDatabaseAccess>().To<SurrealDatabaseAccess>().InSingletonScope();
            //this.Bind<IDatabaseAccess>().To<DatabaseAccessMock>();
        }
    }
}
