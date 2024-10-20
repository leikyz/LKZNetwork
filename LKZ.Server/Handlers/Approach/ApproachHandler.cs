using LKZ.Network.Common.Events;
using LKZ.Server.Managers; // Assurer d'inclure le namespace du LobbyManager et PlayerManager
using LKZ.Server.Network;
using LKZ.Server.Network.Objects;
using System;
using System.Collections.Generic;

namespace LKZ.Network.Server.Handlers.Approach
{
    static public class ApproachHandler
    {
        static public void HandleOnlinePlayerCountMessage(BaseClient client, string[] parameters)
        {
            BaseServer.TriggerClientEvent((int)client.Id, "OnlinePlayerCountMessage", -1, BaseServer.ClientsCount);
        }

        static public void HandleLobbyCreatedMessage(BaseClient client, string[] parameters)
        {
            var lobby = LobbyManager.CreateLobby();
            //lobby.AddClient(client);
            //client.Lobby = lobby;

            Console.WriteLine($"Lobby '{lobby.LobbyId}' created.");

            BaseServer.TriggerGlobalEvent("LobbyCreatedMessage", lobby.LobbyId);
           
        }

        static public void HandleLobbyJoinedMessage(BaseClient client, string[] parameters)
        {
            int lobbyId = int.Parse(parameters[0]);

            Lobby lobby = LobbyManager.GetLobby(lobbyId);
            lobby.AddClient(client);
            client.Lobby = lobby;

            BaseServer.TriggerClientEvent((int)client.Id, "LobbyJoinedMessage", lobby.LobbyId, lobbyId);

        }

        static public void HandleLobbyListMessage(BaseClient client, string[] parameters)
        {
            List<Lobby> allLobbies = LobbyManager.GetAllLobbies();

            List<string> lobbyInfoList = new List<string>();
            foreach (var lobby in allLobbies)
            {
                lobbyInfoList.Add($"{lobby.LobbyId}^{lobby.ClientsCount}");
            }
            string lobbiesString = string.Join("$", lobbyInfoList);


            //send to a specific client (no matter lobby)
            BaseServer.TriggerClientEvent((int)client.Id, "LobbyListMessage", -1, lobbiesString);

        }

    }
}
