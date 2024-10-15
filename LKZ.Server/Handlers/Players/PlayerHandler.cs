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
    }
}