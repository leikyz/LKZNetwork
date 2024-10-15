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
            }
            else
            {
            }
        }

        public static void RemovePlayer(int playerId)
        {
            if (connectedPlayers.Contains(playerId))
            {
                connectedPlayers.Remove(playerId);
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
                return index; // Retourne l'index du joueur
            }
            else
            {
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
