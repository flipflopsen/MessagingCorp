using MessagingCorp.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        var service = new MessageCorpService();
        service.InitializeService();

        await service.StartOperation();

        var input = Console.ReadLine();
    }
}