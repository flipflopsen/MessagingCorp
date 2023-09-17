using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.BO
{
    public class Lobby
    {
        private readonly int _lobbyId;
        public string LobbyName { get; set; }
        private List<User> _usersInLobby;
        private List<CorpMessage> _messagesInLobby;
        private ConcurrentQueue<CorpMessage> _messagesToDeliver;

        public Lobby()
        {
            _usersInLobby = new ();
            _messagesInLobby = new ();
            _messagesToDeliver = new ();
        }
    }
}
