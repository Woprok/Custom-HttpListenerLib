using System;
using System.Net;
using Shared.Networking.Advanced.DataModels;
using Shared.Networking.Models.Models.StreamModels;

namespace Demo.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var rrr = new SocketModelExample("http://localhost:8080/hello/", new JsonSerializer());
            //var server = new HttpListenerSocketServer();
            //server.Start("http://localhost:8080/hello/");

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