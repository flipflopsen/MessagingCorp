using MessagingCorp.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var service = new MessageCorpService();
        service.InitializeService();
    }
}