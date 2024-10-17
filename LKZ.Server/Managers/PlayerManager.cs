using System;
using System.Collections.Generic;

namespace LKZ.Server.Managers
{
    public static class PlayerManager
    {
        // Dictionnaire de tous les joueurs connectés par leur ID
        private static Dictionary<int, int> playersInLobbies = new Dictionary<int, int>(); // <PlayerId, LobbyId>

        // Ajouter un joueur avec son lobby associé
        public static void AddPlayer(int playerId, int lobbyId)
        {
            if (!playersInLobbies.ContainsKey(playerId))
            {
                playersInLobbies.Add(playerId, lobbyId);
                Console.WriteLine($"Player {playerId} added to Lobby {lobbyId}");
            }
            else
            {
                Console.WriteLine($"Player {playerId} already in a lobby.");
            }
        }

        // Retirer un joueur
        public static void RemovePlayer(int playerId)
        {
            if (playersInLobbies.ContainsKey(playerId))
            {
                playersInLobbies.Remove(playerId);
                Console.WriteLine($"Player {playerId} removed.");
            }
            else
            {
                Console.WriteLine($"Player {playerId} not found.");
            }
        }

        // Récupérer le lobby d'un joueur
        public static int GetPlayerLobby(int playerId)
        {
            if (playersInLobbies.TryGetValue(playerId, out int lobbyId))
            {
                return lobbyId;
            }
            return -1; // Retourne -1 si le joueur n'est pas trouvé
        }

        // Vérifier si un joueur est dans un lobby
        public static bool IsPlayerInLobby(int playerId)
        {
            return playersInLobbies.ContainsKey(playerId);
        }

        // Récupérer tous les joueurs
        public static Dictionary<int, int> GetAllPlayers()
        {
            return new Dictionary<int, int>(playersInLobbies); // Retourne une copie des joueurs
        }
    }
}
