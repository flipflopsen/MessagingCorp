using MessagingCorp.Services;
using MessagingCorp.Utils.Logger;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

public class Program
{
    public static void Main(string[] args)
    {
        var service = new MessageCorpService();
        service.InitializeService();
    }
}