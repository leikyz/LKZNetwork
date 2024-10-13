using LKZ.Network.Common.Events;
using LKZ.Server.Handlers.Chat;
using LKZ.Server.Handlers.Players;
using System;
using System.Collections.Generic;
using System.IO;
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

            RegisterEvents();
        }

        private static void RegisterEvents()
        {
            EventManager.RegisterEvent("PlayerCreated", PlayerHandler.HandlePlayerCreatedMessage);
            EventManager.RegisterEvent("SendPrivateChatMessage", ChatHandler.HandleChatMessageMessage);
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
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];

                while (client.Connected)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                        break; // if there is no data

                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    OnDataReceived?.Invoke(null, message, client);
                }
            }

            OnClientDisconnected?.Invoke(null, client);
        }

        public static void HandleClientConnected(object sender, TcpClient client)
        {
            Console.WriteLine($"TCPClient {client.Client.RemoteEndPoint} connected.");
        }

        public static void HandleClientDisconnected(object sender, TcpClient client)
        {
            Console.WriteLine("A client has disconnected.");
        }

        private static void HandleDataReceived(object sender, string message, TcpClient client)
        {
            var parts = message.Split('|');

            if (parts[0] == "ClientCreatedMessage")
            {
                AddClient(Int32.Parse(parts[1]), client);
            }

            EventManager.TriggerRaw(message);
            Console.WriteLine($"Message received ({parts[1]}): {parts[0]} ({parts[2]})");
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

        public static TcpClient GetTcpClient(int id)
        {
            if (clients.TryGetValue(id, out TcpClient client))
            {
                return client;
            }
            else
            {
                Console.WriteLine($"Client with ID {id} not found.");
                return null;
            }
        }

        public static void TriggerClientEvent(int clientId, string eventName, params object[] parameters)
        {
            TcpClient tcpClient = GetTcpClient(clientId);
            string paramStr = string.Join(",", parameters);
            string fullMessage = $"{eventName}|{clientId}|{paramStr}"; // Change the order

            byte[] data = Encoding.ASCII.GetBytes(fullMessage);
            tcpClient.GetStream().Write(data, 0, data.Length);
           // Thread.Sleep(TimeBetweenMessage);
        }

        public static void TriggerClientsEvent(int clientId, string eventName, params object[] parameters)
        {
            string paramStr = string.Join(",", parameters);
            string fullMessage = $"{eventName}|{clientId}|{paramStr}"; // Change the order

            byte[] data = Encoding.ASCII.GetBytes(fullMessage);

            foreach (var client in clients)
            {
                client.Value.GetStream().Write(data, 0, data.Length);
            }
        }

        public static void ListClients()
        {
            if (clients.Count == 0)
            {
                Console.WriteLine("No clients connected.");
                return;
            }

            Console.WriteLine("Connected clients:");
            foreach (var client in clients)
            {
                Console.WriteLine($"Client ID: {client.Key}, Remote EndPoint: {client.Value.Client.RemoteEndPoint}");
            }
        }
    }
}
