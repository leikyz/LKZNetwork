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

    // parameters[0] = event name
    // parameters[1] = client id
    // other = parameters
    public static class BaseServer
    {
        public const int MAX_BUFFER_SIZE = 1024;

        private static TcpListener _server;
        private static bool _isRunning;
        private static Dictionary<uint, TcpClient> clients = new Dictionary<uint, TcpClient>();

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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"({ipAddress} {port}) Server started, waiting for clients...");
            Console.ResetColor();
            OnClientConnected += HandleClientConnected;
            OnClientDisconnected += HandleClientDisconnected;
            OnDataReceived += HandleDataReceived;
            
            // Continuously accept new clients in a loop
            Task.Run(() => AcceptClients());

            RegisterEvents();
        }

        private static void RegisterEvents()
        {
            EventManager.RegisterEvent("PlayerCreatedMessage", PlayerHandler.HandlePlayerCreatedMessage);
            EventManager.RegisterEvent("PlayerMovedMessage", PlayerHandler.HandlePlayerMovedMessage);
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
                byte[] buffer = new byte[MAX_BUFFER_SIZE];

                while (client.Connected)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                        break; // if there is no data

                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

  
                    if (message.Length > MAX_BUFFER_SIZE) 
                    {
                        Console.WriteLine($"Error: Received message is too large ({message.Length} characters).");
                        continue; // Ignore ce message et passe au suivant
                    }

                    OnDataReceived?.Invoke(null, message, client);
                }
            }

            OnClientDisconnected?.Invoke(null, client);
        }


        public static void HandleClientConnected(object sender, TcpClient client)
        {
           
        }

        public static void HandleClientDisconnected(object sender, TcpClient client)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("A client has disconnected.");
            Console.ResetColor();
        }

        private static void HandleDataReceived(object sender, string message, TcpClient client)
        {

            string[] messages = message.Split('µ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var msg in messages)
            {
                var parts = msg.Split('|');

                if (parts[0] == "ClientCreatedMessage")
                {
                    AddClient(UInt32.Parse(parts[1]), client);
                }

                EventManager.TriggerRaw(msg);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"({parts[1]}) Message received : {parts[0]} ({parts[2]})");
                Console.ResetColor();
            }
        }


        public static void AddClient(uint id, TcpClient client)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (!clients.ContainsKey(id))
                {
                    clients.Add(id, client);
                   
                    Console.WriteLine($"({client.Client.RemoteEndPoint}) Client with ID {id} connected successfully.");
                }
                else
                {
                    Console.WriteLine($"Client with ID {id} already exists.");
                }

                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding client with ID {id}: {ex.Message}");
            }
        }

        public static TcpClient GetTcpClient(uint id)
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
        /// <summary>
        /// Triggers an event for the client:
        /// clientId > 0 = sends the event to the specified client.
        /// clientId = -2 = sends the event to all clients except the client in the first parameter.
        ///clientId = -1 = sends the event to all clients.
        /// </summary>
        public static void TriggerClientEvent(int clientId, string eventName, params object[] parameters)
        {
            // Convertir les paramètres en chaîne de caractères
            string paramStr = string.Join(",", parameters);
            string fullMessage = $"{eventName}|{clientId}|{paramStr}"; // Change the order
            byte[] data = Encoding.ASCII.GetBytes(fullMessage);

            // Cas où l'on envoie à tous les clients (clientId == -1)
            if (clientId == -1)
            {
                foreach (var client in clients)
                {
                    client.Value.GetStream().Write(data, 0, data.Length);
                }
            }
            // Cas où l'on envoie à tous les clients sauf un (clientId == -2)
            else if (clientId == -2 && parameters.Length > 0 && parameters[0] is int excludeClientId)
            {
                foreach (var client in clients)
                {
                    if (client.Key != excludeClientId) // Ne pas envoyer au client spécifié en premier paramètre
                    {
                        client.Value.GetStream().Write(data, 0, data.Length);
                    }
                }
            }
            // Cas où l'on envoie à un client spécifique (clientId > 0)
            else if (clientId > 0)
            {
                TcpClient tcpClient = GetTcpClient((uint)clientId);

                if (tcpClient != null)
                {
                    tcpClient.GetStream().Write(data, 0, data.Length);
                
                }
                else
                {
                    Console.WriteLine($"Client {clientId} not found.");
                }
            }
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"({clientId}) Message sent : {eventName} ({paramStr})");
            Console.ResetColor();
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
