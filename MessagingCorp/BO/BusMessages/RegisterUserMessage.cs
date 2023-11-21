using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.BO.BusMessages
{
    public class RegisterUserMessage
    {
        public RegisterUserMessage(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set;}
        public string Password { get; set;}
    }
}
