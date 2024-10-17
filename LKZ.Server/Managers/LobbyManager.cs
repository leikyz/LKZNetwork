using System;
using System.Collections.Generic;

namespace LKZ.Server.Managers
{
    public class Lobby
    {
        public int LobbyId { get; private set; }
        private List<int> clients;

        public Lobby(int id)
        {
            LobbyId = id;
            clients = new List<int>();
        }

        public void AddClient(int playerId)
        {
            if (!clients.Contains(playerId))
            {
                clients.Add(playerId);
                PlayerManager.AddPlayer(playerId, LobbyId); // Associer le joueur à ce lobby
            }
        }

        public void RemovePlayer(int playerId)
        {
            if (clients.Contains(playerId))
            {
                clients.Remove(playerId);
                PlayerManager.RemovePlayer(playerId); // Supprimer aussi le joueur du PlayerManager
            }
            else
            {
                Console.WriteLine($"Player {playerId} not found in lobby {LobbyId}.");
            }
        }

        public List<int> GetPlayers()
        {
            return new List<int>(clients);
        }

        public bool IsPlayerInLobby(int playerId)
        {
            return clients.Contains(playerId);
        }
    }

    public static class LobbyManager
    {
        private static List<Lobby> lobbies = new List<Lobby>();
        private static int nextLobbyId = 5; // ID de lobby incrémenté automatiquement

        // Création d'un nouveau lobby avec un ID unique
        public static Lobby CreateLobby()
        {
            int newLobbyId = nextLobbyId++; // Incrémentation automatique de l'ID
            Lobby newLobby = new Lobby(newLobbyId);
            lobbies.Add(newLobby);
            Console.WriteLine($"Lobby {newLobbyId} created.");
            return newLobby;
        }

        // Supprimer un lobby
        public static void RemoveLobby(int lobbyId)
        {
            Lobby lobbyToRemove = GetLobby(lobbyId);
            if (lobbyToRemove != null)
            {
                lobbies.Remove(lobbyToRemove);
                Console.WriteLine($"Lobby {lobbyId} removed.");
            }
            else
            {
                Console.WriteLine($"Lobby {lobbyId} not found.");
            }
        }

        // Récupérer un lobby par son ID
        public static Lobby GetLobby(int lobbyId)
        {
            return lobbies.Find(lobby => lobby.LobbyId == lobbyId);
        }

        // Vérifier si un lobby existe
        public static bool IsLobbyExists(int lobbyId)
        {
            return lobbies.Exists(lobby => lobby.LobbyId == lobbyId);
        }

        // Obtenir tous les lobbies
        public static List<Lobby> GetAllLobbies()
        {
            return new List<Lobby>(lobbies);
        }

        // Ajouter un joueur à un lobby spécifique
        public static void AddPlayerToLobby(int playerId, int lobbyId)
        {
            Lobby lobby = GetLobby(lobbyId);
            if (lobby != null)
            {
                lobby.AddClient(playerId);
            }
            else
            {
                Console.WriteLine($"Lobby {lobbyId} not found.");
            }
        }

        // Retirer un joueur d'un lobby spécifique
        public static void RemovePlayerFromLobby(int playerId, int lobbyId)
        {
            Lobby lobby = GetLobby(lobbyId);
            if (lobby != null)
            {
                lobby.RemovePlayer(playerId);
            }
            else
            {
                Console.WriteLine($"Lobby {lobbyId} not found.");
            }
        }
    }
}
