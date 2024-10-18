using LKZ.Network.Common.Events;
using LKZ.Server.Managers; // Assurer d'inclure le namespace du LobbyManager et PlayerManager
using LKZ.Server.Network;
using System;
using System.Collections.Generic;

namespace LKZ.Network.Server.Handlers.Approach
{
    static public class ApproachHandler
    {
        static public void HandleLobbyCreatedMessage(string[] parameters)
        {
            // On s'assure que les paramètres contiennent l'information nécessaire
            //if (parameters.Length < 2)
            //{
            //    Console.WriteLine("Invalid parameters for LobbyCreatedMessage.");
            //    return;
            //}

            var lobby = LobbyManager.CreateLobby();
            lobby.AddClient(int.Parse(parameters[0]));

            Console.WriteLine($"Lobby '{lobby.LobbyId}' created.");

            BaseServer.TriggerClientEvent(-1, "LobbyCreatedMessage", lobby.LobbyId);
        }

        static public void HandleLobbyListMessage(string[] parameters)
        {
            //List<Lobby> allLobbies = LobbyManager.GetAllLobbies();

            //List<string> lobbyInfoList = new List<string>();
            //foreach (var lobby in allLobbies)
            //{
            //    lobbyInfoList.Add($"{lobby.LobbyId}^{lobby.GetPlayers().Count}");
            //}
            ////lobbyInfoList.Add("5!1");
            //string lobbiesString = string.Join("$", lobbyInfoList);

            //BaseServer.TriggerClientEvent(int.Parse(parameters[0]), "LobbyListMessage", lobbiesString);

            //Console.WriteLine($"Lobby list sent: {lobbiesString}");
        }

    }
}
