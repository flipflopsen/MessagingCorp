using MessagingCorp.BO;
using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Services;
using MessagingCorp.Services.API;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using Serilog.Events;
using Serilog;
using SurrealDb;
using SurrealDb.Net;
using MessagingCorp.Utils.Logger;
using MessagingCorp.Database.DAO;
using MessagingCorp.Utils.Converters;

namespace MessagingCorp.Database
{
    public class SurrealDatabaseAccess : IDatabaseAccess
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<SurrealDatabaseAccess>("./Logs/MessageCorpDatabase.log", true, LogEventLevel.Debug);

        private SurrealDbClient _client;

        [Inject]
        public SurrealDatabaseAccess(IMessageCorpConfiguration config)
        {
            var dbconf = (DatabaseConfiguration)config.GetConfiguration(MessageCorpConfigType.Database);

            var endpoint = dbconf.Server + ":" + dbconf.Port;
            var options = SurrealDbOptions.Create()
            .WithEndpoint(endpoint)
            .WithDatabase(dbconf.DatabaseName)
            .WithNamespace(dbconf.NameSpace)
            .WithUsername(dbconf.Username)
            .WithPassword(dbconf.Password)
            .Build();

            _client = new SurrealDbClient(options);
        }

        public async Task AddUser(string uid, string username, string pass)
        {
            await _client.Connect();
            if (await IsUidExistent(uid))
            {
                Logger.Error("Tried to create a user with already existing uid!");
                //TODO: Custom exception handling
                return;
            }
            await _client.Create<UserRecordDao>("user:" + uid, new UserRecordDao(uid, username, pass, new List<string>() { "123" }));
            Logger.Information("Created user with uid: " + uid);
        }

        public async Task<bool> AuthenticateUser(string uid, string password)
        {
            await _client.Connect();
            var usr = await GetUser(uid);
            return usr.Password == password;
        }

        public async Task<User> GetUser(string uid)
        {
            await _client.Connect();
            var sel = await _client.Select<UserRecordDao>($"user:{uid}");
            var first = (sel.FirstOrDefault(f => f.UserId!.Equals(uid)));
            if (first != null)
            {
                Logger.Information("Got user with uid: " + first.UserId);
                var conv = new StaticDaoToBoConverter<UserRecordDao, User>();

                var usr = conv.Convert(first);

                return usr;
            }

            Logger.Warning("Failed to get user with uid: " + uid);
            return null!;
        }

        public async Task<bool> IsUidExistent(string uid)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveUser(string uid)
        {
            throw new NotImplementedException();
        }
    }
}
