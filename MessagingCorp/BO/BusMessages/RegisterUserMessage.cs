using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.BO.BusMessages
{
    public class RegisterUserMessage
    {
        public string Uid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public RegisterUserMessage(string uid, string userName, string password)
        {
            Uid = uid;
            UserName = userName;
            Password = password;
        }
    }
}
