using Autofac;
using MessagingCorp.Configuration;
using MessagingCorp.Database;
using MessagingCorp.Database.API;
using MessagingCorp.EntityManagement;
using MessagingCorp.EntityManagement.API;
using MessagingCorp.Modules;
using MessagingCorp.Services.API;
using MessagingCorp.Services.Core;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.ApplicationStartup
{
    public static class AutofacStartup
    {
        private static IContainer Container { get; set; }

        public static void Startup()
        {
            // TODO: Remove ninject usage here, 
            var kernel = new StandardKernel(
                    new MessageCorpServiceModule(),
                    new CryptoModule(),
                    new CachingModule(),
                    new DatabaseModule(false),
                    new AuthenticationModule(),
                    new UserManagementModule()
                    );

            var builder = new ContainerBuilder();

            builder.RegisterType<StandardKernel>().As<IKernel>();
            builder.RegisterType<DatabaseAccessMock>().As<IDatabaseAccess>().PropertiesAutowired();
            //builder.Register(c => new UserManagement(c.Resolve<IKernel>())).As<IUserManagement>().PropertiesAutowired();

            builder.RegisterType<MessageFacService>().As<IMessageFacService>();

            Container = builder.Build();

            using (var scope = Container.BeginLifetimeScope())
            {
                var service = scope.Resolve<IMessageFacService>();
                service.Test("123");
            }
        }
    }
}
