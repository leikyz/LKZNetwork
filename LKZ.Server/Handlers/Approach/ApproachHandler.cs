using LKZ.Network.Common.Events;
using LKZ.Server.Managers; // Assurer d'inclure le namespace du LobbyManager et PlayerManager
using LKZ.Server.Network;
using System;
using System.Collections.Generic;

namespace LKZ.Network.Server.Handlers.Approach
{
    static public class ApproachHandler
    {
        static public void HandleLobbyCreatedMessage(BaseClient client, string[] parameters)
        {
            var lobby = LobbyManager.CreateLobby();
            lobby.AddClient(client);
            client.Lobby = lobby;

            Console.WriteLine($"Lobby '{lobby.LobbyId}' created.");

            BaseServer.TriggerClientEvent(-1, "LobbyCreatedMessage", lobby.LobbyId, lobby.LobbyId);
           
        }

        static public void HandleLobbyListMessage(BaseClient client, string[] parameters)
        {
            //List<Lobby> allLobbies = LobbyManager.GetAllLobbies();

            //List<string> lobbyInfoList = new List<string>();
            //foreach (var lobby in allLobbies)
            //{
            //    lobbyInfoList.Add($"{lobby.LobbyId}^{lobby.ClientsCount}");
            //}
            //string lobbiesString = string.Join("$", lobbyInfoList);

            //BaseServer.TriggerClientEvent(int.Parse(parameters[0]), "LobbyListMessage", lobbiesString);

        }

    }
}
