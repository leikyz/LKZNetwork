using LKZ;
using LKZ.Network.Common.Events;
using LKZ.Server.Network;
using System;
using System.Threading;

namespace LKZ.Network.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            string[] lines = new string[]
        {
            @"              _    _  __ ____           ",
            @"             | |  | |/ /|_  /           ",
            @"             | |__| ' <  / /            ",
            @"  _  _ ___ __|____|_|\_\/___|  ___ _  __",
            @" | \| | __|_   _\ \    / / _ \| _ \ |/ /",
            @" | .` | _|  | |  \ \/\/ / (_) |   / ' < ",
            @" |_|\_|___| |_|   \_/\_/ \___/|_|_\_|\_\",
            @"                                         ",
            @"                                         ",
            @"                                         "
        };
            int consoleWidth = Console.WindowWidth;

            foreach (string line in lines)
            {
                int padding = (consoleWidth - line.Length) / 2;
                Console.WriteLine(line.PadLeft(line.Length + padding));
            }

            Console.ResetColor();

            Thread serverThread = new Thread(() => BaseServer.Start("127.0.0.1", 5000));
            serverThread.Start();

            

            Thread.Sleep(1000);
            Console.ReadLine();

            BaseServer.Stop();
        }
    }
}
