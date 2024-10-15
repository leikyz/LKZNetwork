using System.Collections.Generic;

namespace LKZ.Server.Managers
{
    static public class LobbyManager
    {
        private static List<int> connectedPlayers = new List<int>();

        public static void AddPlayer(int playerId)
        {
            if (!connectedPlayers.Contains(playerId))
            {
                connectedPlayers.Add(playerId);
                Console.WriteLine($"Player {playerId} added.");
            }
            else
            {
                Console.WriteLine($"Player {playerId} is already connected.");
            }
        }

        public static void RemovePlayer(int playerId)
        {
            if (connectedPlayers.Contains(playerId))
            {
                connectedPlayers.Remove(playerId);
                Console.WriteLine($"Player {playerId} removed.");
            }
            else
            {
                Console.WriteLine($"Player {playerId} not found.");
            }
        }
        public static int GetPlayerPosition(int playerId)
        {
            int index = connectedPlayers.IndexOf(playerId);
            if (index >= 0)
            {
                Console.WriteLine($"Player {playerId} is at position {index}.");
                return index; // Retourne l'index du joueur
            }
            else
            {
                Console.WriteLine($"Player {playerId} not found in the list.");
                return -1; // Retourne -1 si le joueur n'est pas trouvé
            }
        }
        public static List<int> GetConnectedPlayers()
        {
            return new List<int>(connectedPlayers); 
        }

        public static bool IsPlayerConnected(int playerId)
        {
            return connectedPlayers.Contains(playerId);
        }
    }
}
