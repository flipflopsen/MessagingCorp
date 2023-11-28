using MessagingCorp.BO.BusMessages;
using MessagingCorp.Common.EventBus;
using MessagingCorp.Common.HttpStuff;
using MessagingCorp.Utils.Converters;
using MessagingCorp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MessagingCorp.Services.API
{
    public interface IMessageCorpController
    {
        Task RunCorpHttp();
    }
}
