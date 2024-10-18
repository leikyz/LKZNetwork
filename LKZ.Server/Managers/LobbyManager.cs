using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace LKZ.Server.Managers
{
    public class Lobby
    {
        public int LobbyId { get; private set; }
        private List<BaseClient> clients;
        private List<Entity> entities;
        public Lobby(int id)
        {
            LobbyId = id;
            clients = new List<BaseClient>();
            entities = new List<Entity>();
        }

        public void AddClient(BaseClient client)
        {
            if (!clients.Contains(client))
            {
                clients.Add(client);
            }
        }

        public void AddEntity(Entity entity)
        {
            if (!entities.Contains(entity))
            {
                entities.Add(entity);
            }
        }

        public void RemovePlayer(BaseClient client)
        {
            if (clients.Contains(client))
            {
                clients.Remove(client);
            }
        }

        public List<BaseClient> Clients => clients;

        public int ClientsCount => clients.Count;  
    }

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
