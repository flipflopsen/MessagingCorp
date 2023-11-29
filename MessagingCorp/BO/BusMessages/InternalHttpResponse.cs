using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.BO.BusMessages
{
    public class InternalHttpResponse
    {
        public bool IsSuccess { get; set; }
        public string Userid { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

    }
}
