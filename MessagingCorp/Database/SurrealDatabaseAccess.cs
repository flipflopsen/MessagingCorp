using MessagingCorp.BO;
using MessagingCorp.Configuration;
using MessagingCorp.Configuration.BO;
using MessagingCorp.Services;
using Microsoft.Extensions.DependencyInjection;
using Ninject;
using Serilog.Events;
using Serilog;
using SurrealDB.Configuration;
using SurrealDB.Driver.Rpc;
using SurrealDB.Models;
using MessagingCorp.Utils.Logger;
using MessagingCorp.Database.DAO;
using MessagingCorp.Utils.Converters;
using SurrealDB.Models.Result;
using SurrealDB.Driver.Rest;
using SurrealDb.Net;
using MessagingCorp.Database.API;

namespace MessagingCorp.Database
{
    public class SurrealDatabaseAccess : IDatabaseAccess
    {
        private static readonly ILogger Logger = Log.Logger.ForContextWithConfig<SurrealDatabaseAccess>("./Logs/MessageCorpDatabase.log", true, LogEventLevel.Debug);

        private readonly SurrealDbClient _client;
        private readonly DatabaseConfiguration _dbConfig;

        private const string USER_TABLE = "Userinos";
        private const string SYM_VAULT = "VaultSym";
        private const string ASYM_VAULT = "VaultAsym";

        [Inject]
        public SurrealDatabaseAccess(IMessageCorpConfiguration config)
        {
            _dbConfig = (DatabaseConfiguration)config.GetConfiguration(MessageCorpConfigType.Database);

            var endpoint = _dbConfig.Server + ":" + _dbConfig.Port;
            
            var options = SurrealDbOptions.Create()
            .WithEndpoint(endpoint)
            .WithDatabase(_dbConfig.UserDatabaseName)
            .WithUsername(_dbConfig.Username)
            .WithPassword(_dbConfig.Password)
            .Build();

            _client = new(options);
        }

        // return surrealId
        public async Task<string> AddUser(string uid, string username, string pass)
        {
            try
            {
                await _client.Connect();
                await _client.Use(_dbConfig.NameSpace, _dbConfig.UserDatabaseName);

                if (await IsUidExistent(uid))
                {
                    Logger.Error("Tried to create a user with already existing uid!");
                    return string.Empty;
                }
                
                var dao = new UserRecordDao(uid, username, pass);

                var created = await _client.Create(USER_TABLE, dao);

                Logger.Information($"Created user with uid: {uid}, and SurrealId: {created.Id!.Id}");

                return created.Id!.Id;
            
            }
            catch (Exception e)
            {
                Logger.Error("Exception thrown: " + e.Message);
                return string.Empty;
            }
        }

        public async Task<bool> AuthenticateUser(string uid, string password)
        {
            var usr = await GetUser(uid);
            return usr.Password == password;
        }

        public async Task<User> GetUser(string uid)
        {
            await _client.Connect();
            await _client.Use(_dbConfig.NameSpace, _dbConfig.UserDatabaseName);

            var select = await _client.Select<UserRecordDao>(USER_TABLE, uid);
            if (select != null)
            {
                var conv = new StaticDaoToBoConverter<UserRecordDao, User>();

                //TODO: Utilize automapper or sth
                var usr = conv.Convert(select);

                return usr;
            }
            Logger.Warning($"Failed to get user with uid: {uid}");
            return null!;
        }

        public async Task<bool> IsUidExistent(string uid)
        {
            return await GetUser(uid) != null;
        }

        public async Task<bool> RemoveUser(string uid)
        {
            await _client.Connect();
            await _client.Use(_dbConfig.NameSpace, _dbConfig.UserDatabaseName);
            return await _client.Delete(USER_TABLE, uid);
        }

        public async Task<IEnumerable<UserRecordDao>> GetAllUsers()
        {
            await _client.Connect();
            await _client.Use(_dbConfig.NameSpace, _dbConfig.UserDatabaseName);

            var users = await _client.Select<UserRecordDao>($"{USER_TABLE}");

            if (users != null && users.Any())
                return users;
            else
                return null!;
        }
    }
}
