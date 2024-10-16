﻿using LKZ.Network.Common.Events;
using LKZ.Network.Server.Handlers.Approach;
using LKZ.Server.Handlers.Entity;
using LKZ.Server.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static List<BaseClient> clients = new List<BaseClient>();
        public static uint NextClientId = 1;
        public static uint NextEntityId = 1;
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
            EventManager.RegisterEvent("LobbyCreatedMessage", ApproachHandler.HandleLobbyCreatedMessage);
            EventManager.RegisterEvent("LobbyListMessage", ApproachHandler.HandleLobbyListMessage);
            EventManager.RegisterEvent("EntityCreatedMessage", EntityHandler.HandleEntityCreatedMessage);
            EventManager.RegisterEvent("LobbyJoinedMessage", ApproachHandler.HandleLobbyJoinedMessage);
            //EventManager.RegisterEvent("PlayerCreatedMessage", PlayerHandler.HandlePlayerCreatedMessage);
            //EventManager.RegisterEvent("PlayerMoveMessage", PlayerHandler.HandlePlayerMoveMessage);
            //EventManager.RegisterEvent("SendPrivateChatMessage", ChatHandler.HandleChatMessageMessage);
            //EventManager.RegisterEvent("PlayerRotationMessage", PlayerHandler.HandlePlayerRotationMessage);
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

        private static void HandleDataReceived(object sender, string message, TcpClient tcpClient)
        {
            string[] messages = message.Split('~', StringSplitOptions.RemoveEmptyEntries);
            foreach (var msg in messages)
            {
                
                var parts = msg.Split('|');

                if (parts[0] == "ClientCreatedMessage")
                {
                    AddClient(new BaseClient(NextClientId, tcpClient));
                    BaseServer.NextClientId++;
                }
                BaseClient client = clients.FirstOrDefault(x => x.TcpClient == tcpClient);


                if (client.Id == 0)
                {
                    Console.WriteLine("Client non trouvé.");
                    return; 
                }



                EventManager.TriggerRaw(client, $"{parts[0]}|{parts[1]}");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"({tcpClient.Client.RemoteEndPoint}) Message received : {parts[0]} (ID: {client.Id}, Parameters: {parts[1]})");
                Console.ResetColor();
            }
        }


        public static void AddClient(BaseClient client)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (!clients.Contains(client))
                {
                    clients.Add(client);
                    Console.WriteLine($"({client.TcpClient.Client.RemoteEndPoint}) Client with ID {client.Id} connected successfully.");
                }
                else
                {
                    Console.WriteLine($"Client with ID {client.Id} already exists.");
                }
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding client with ID {client.Id}: {ex.Message}");
            }
        }


        public static BaseClient GetClient(uint id)
        {
            BaseClient client = clients.FirstOrDefault(c => c.Id == id);

            if (client != null)
            {
                return client;
            }
            else
            {
                Console.WriteLine($"Client with ID {id} not found.");
                return null;
            }
        }


        //public static TcpClient GetTcpClient(string id)
        //{
        //    if (clients.TryGetValue(id, out TcpClient client))
        //    {
        //        return client;
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Client with ID {id} not found.");
        //        return null;
        //    }
        //}
        /// <summary>
        /// Triggers an event for the client:
        /// clientId > 0 = sends the event to the specified client.
        /// clientId = -2 = sends the event to all clients except the client in the first parameter.
        ///clientId = -1 = sends the event to all clients.
        /// </summary>
        /// 

        public static void TriggerGlobalEvent(string eventName, params object[] parameters)
        {
            string fullMessage = EventManager.Serialize(eventName, parameters);
            Debug.WriteLine(fullMessage);
            byte[] data = Encoding.ASCII.GetBytes(fullMessage);

            // Iterate over all connected clients and send the message
            foreach (var client in clients)
            {
                try
                {
                    client.TcpClient.GetStream().Write(data, 0, data.Length);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                 //   Console.WriteLine($"Message sent to client {client.Id}: {eventName} ({parameters.Length})");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending message to client {client.Id}: {ex.Message}");
                }
            }
        }

        public static void TriggerClientEvent(int clientId, string eventName, int lobbyId = -1, params object[] parameters)
        {
            string fullMessage = EventManager.Serialize(eventName, parameters);
            Debug.WriteLine(fullMessage);
            byte[] data = Encoding.ASCII.GetBytes(fullMessage);

            // Check if a valid lobby ID is provided
            if (lobbyId > 0)
            {
                Lobby lobby = LobbyManager.GetLobby(lobbyId);
                if (lobby != null)
                {
                    // Case 1: Send to all clients in the specified lobby
                    if (clientId == -1)
                    {
                        foreach (var client in lobby.Clients)
                        {
                            client.TcpClient.GetStream().Write(data, 0, data.Length);
                        }
                    }
                    // Case 2: Send to a specific client in the lobby
                    else if (clientId > 0)
                    {
                        var targetClient = lobby.Clients.FirstOrDefault(c => c.Id == clientId);
                        if (targetClient != null)
                        {
                            targetClient.TcpClient.GetStream().Write(data, 0, data.Length);
                        }
                        else
                        {
                            Console.WriteLine($"Client {clientId} not found in lobby {lobbyId}.");
                        }
                    }
                    // Case 3: Send to all clients in the lobby except the specified one
                    else if (clientId == -2 && parameters.Length > 0 && parameters[0] is int excludeClientId)
                    {
                        foreach (var client in lobby.Clients)
                        {
                            if (client.Id != excludeClientId)
                            {
                                client.TcpClient.GetStream().Write(data, 0, data.Length);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Lobby {lobbyId} not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid lobby ID.");
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"({clientId}) Message sent: {eventName} ({parameters.Length})");
            Console.ResetColor();
        }



        //public static void ListClients()
        //{
        //    if (clients.Count == 0)
        //    {
        //        Console.WriteLine("No clients connected.");
        //        return;
        //    }

        //    Console.WriteLine("Connected clients:");
        //    foreach (var client in clients)
        //    {
        //        Console.WriteLine($"Client ID: {client.Key}, Remote EndPoint: {client.Value.Client.RemoteEndPoint}");
        //    }
        //}
    }
}
