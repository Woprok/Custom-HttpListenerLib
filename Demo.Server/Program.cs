using System;

namespace Demo.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new HttpListenerSocketServer();
            server.Start("http://localhost:8080/hello/");

            ConsoleHelpers.KeepRunning();
        }
    }

    class ConsoleHelpers
    {
        public static void KeepRunning()
        {
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}