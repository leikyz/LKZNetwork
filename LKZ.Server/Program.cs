using LKZ;
using LKZ.Network.Common.Events;
using LKZ.Server.Network;

namespace LKZ.Network.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread serverThread = new Thread(() => BaseServer.Start("127.0.0.1", 5000));
            serverThread.Start();


            //EventManager.RegisterEvent("ClientCreatedMessage", Handlers.Approach.ApproachHandler.HandleClientCreatedMessage);
            // Give the server a moment to start
            Thread.Sleep(1000);

            //LKZ.Network.Server.TCP.Network.

            //  LKZ.Network.Server.TCP.Network.TriggerClientEvent(LKZ.Network.Server.TCP.Network.GetClient(), "helo", t)
           

            Console.ReadLine();

        }
    }
}