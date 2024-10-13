using LKZ;
using LKZ.Network.Common.Events;
using LKZ.Server.Handlers.Chat;
using LKZ.Server.Network;
using System;
using System.Threading;

namespace LKZ.Network.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread serverThread = new Thread(() => BaseServer.Start("127.0.0.1", 5000));
            serverThread.Start();

            EventManager.RegisterEvent("SendChatMessage", ChatHandler.HandleChatMessageMessage);

            Thread.Sleep(1000);
            Console.ReadLine();

            BaseServer.Stop();
        }
    }
}
