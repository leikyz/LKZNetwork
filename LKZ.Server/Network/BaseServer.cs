using LKZ.Network.Common.Events;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.Server.Network
{
    public static class BaseServer
    {
        private static TcpListener _server;
        private static bool _isRunning;
        private static Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();

        // Event delegates
        public delegate void ClientConnectedEventHandler(object sender, TcpClient client);
        public delegate void ClientDisconnectedEventHandler(object sender, TcpClient client);
        public delegate void DataReceivedEventHandler(object sender, string message, TcpClient client);

        // Events
        public static event ClientConnectedEventHandler OnClientConnected;
        public static event ClientDisconnectedEventHandler OnClientDisconnected;
        public static event DataReceivedEventHandler OnDataReceived;

        public static void Start(string ipAddress, int port)
        {
            _server = new TcpListener(IPAddress.Parse(ipAddress), port);
            _server.Start();
            _isRunning = true;
            Console.WriteLine("Server started, waiting for clients...");

            OnClientConnected += HandleClientConnected;
            OnClientDisconnected += HandleClientDisconnected;
            OnDataReceived += HandleDataReceived;

            // Continuously accept new clients in a loop
            Task.Run(() => AcceptClients());
        }

        private static async Task AcceptClients()
        {
            while (_isRunning)
            {
                var client = await _server.AcceptTcpClientAsync();

                // Raise OnClientConnected event
                OnClientConnected?.Invoke(null, client);

                // Handle each client in a separate task
                Task.Run(() => HandleClient(client));
            }
        }

        public static void Stop()
        {
            _isRunning = false;
            _server.Stop();
            Console.WriteLine("Server stopped.");
        }

        private static void HandleClient(TcpClient client)
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    // Continuously read from the client
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                        // Raise OnDataReceived event
                        OnDataReceived?.Invoke(null, message, client);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling client: " + ex.Message);
            }
            finally
            {
                // Raise OnClientDisconnected event
                OnClientDisconnected?.Invoke(null, client);
                Console.WriteLine("Client disconnected.");
                client.Close();
            }
        }

        // Event handler for client connected
        public static void HandleClientConnected(object sender, TcpClient client)
        {
            // You can log or manage connected clients here
            Console.WriteLine($"TCPClient {client.Client.RemoteEndPoint} connected.");
        }

        // Event handler for client disconnected
        public static void HandleClientDisconnected(object sender, TcpClient client)
        {
            Console.WriteLine("A client has disconnected.");
        }

        // Event handler for data received
        private static void HandleDataReceived(object sender, string message, TcpClient client)
        {
            var parts = message.Split('|');

            if (parts[1] == "ClientCreatedMessage")
            {
                AddClient(Int32.Parse(parts[0]), client);
            }

            EventManager.TriggerRaw(message);
            Console.WriteLine($"Message received ({parts[0]}) : {parts[1] + " " + "(" + parts[2]}" + ")");
        }

        public static void AddClient(int id, TcpClient client)
        {
            try
            {
                if (!clients.ContainsKey(id))
                {
                    clients.Add(id, client);
                    Console.WriteLine($"Client with ID {id} added successfully.");
                }
                else
                {
                    Console.WriteLine($"Client with ID {id} already exists.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding client with ID {id}: {ex.Message}");
            }
        }

    }
}
