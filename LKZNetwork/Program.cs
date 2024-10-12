using LKZ.Network.Client.Network;
using System;
using System.Net.Sockets;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        // Start the server in a separate thread


        // Connect a client
        BaseClient client = new BaseClient(1);
        client.Connect("127.0.0.1", 5000);
        client.TriggerServerEvent(1, "ClientCreatedMessage");

        BaseClient client2 = new BaseClient(2);
        client.Connect("127.0.0.1", 5000);
        client.TriggerServerEvent(2, "ClientCreatedMessage");

        Console.ReadLine();

        //// Receive response
        //string response = client.ReceiveMessage();

        // Cleanup
        //client.Disconnect();

        //// Stop the server after the client disconnects
        //server.Stop();
        //serverThread.Join(); // Wait for the server thread to finish
    }
}
