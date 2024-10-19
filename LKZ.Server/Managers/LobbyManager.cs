using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using LKZ.Server.Network.Objects;

namespace LKZ.Server.Managers
{
  
    public static class LobbyManager
    {
        private static List<Lobby> lobbies = new List<Lobby>();
        private static int nextLobbyId = 1; 
        public static Lobby CreateLobby()
        {
            Lobby newLobby = new Lobby(nextLobbyId);
            lobbies.Add(newLobby);
            nextLobbyId++;
            return newLobby;
        }

        // Supprimer un lobby
        public static void RemoveLobby(int lobbyId)
        {
            Lobby lobbyToRemove = GetLobby(lobbyId);
            if (lobbyToRemove != null)
            {
                lobbies.Remove(lobbyToRemove);
            }
            else
            {
            }
        }

        public static Lobby GetLobby(int lobbyId)
        {
            return lobbies.Find(lobby => lobby.LobbyId == lobbyId);
        }

        public static bool IsLobbyExists(int lobbyId)
        {
            return lobbies.Exists(lobby => lobby.LobbyId == lobbyId);
        }

        public static List<Lobby> GetAllLobbies()
        {
            return new List<Lobby>(lobbies);
        }
    }
}
