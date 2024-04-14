using simple_authentication_client_console.Implementations;

namespace simple_authentication_client_console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 1)
            {
                try
                {
                    await new ViewController(new HttpService(args[0])).MainLoop();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e}");
                    throw;
                }
            }
            else
            {
                Console.WriteLine("No webApiUrl provided");
            }
        }
    }
}
