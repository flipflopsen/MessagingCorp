using MessagingCorp.Database;
using MessagingCorp.Database.API;
using Ninject.Modules;


namespace MessagingCorp.Modules
{
    public class DatabaseModule : NinjectModule
    {
        private readonly bool useMock;
        public DatabaseModule(bool useMock)
        {
            this.useMock = useMock;
        }

        public override void Load()
        {
            if (this.useMock)
                this.Bind<IDatabaseAccess>().To<DatabaseAccessMock>().InSingletonScope();
            else
                 this.Bind<IDatabaseAccess>().To<SurrealDatabaseAccess>().InSingletonScope();
        }
    }
}
