using LKZ.Server.Network;
using LKZ.Server.Managers;
namespace LKZ.Server.Handlers.Players
{
    static public class PlayerHandler
    {
        static public void HandlePlayerCreatedMessage(string[] parameters)
        {

            int sendId = Int32.Parse(parameters[1]);

            LobbyManager.AddPlayer(sendId);

            BaseServer.TriggerClientEvent(sendId, "PlayerCreatedMessage",LobbyManager.GetPlayerPosition(sendId));
            BaseServer.TriggerClientEvent(-2, "PlayerJoinedMessage", sendId, LobbyManager.GetPlayerPosition(sendId));
        }

        static public void HandlePlayerMovedMessage(string[] parameters)
        {

            int sendId = Int32.Parse(parameters[1]);

            BaseServer.TriggerClientEvent(-2, "PlayerMovedMessage", sendId, parameters[2], parameters[3], parameters[4], parameters[5]);
        }
    }
}