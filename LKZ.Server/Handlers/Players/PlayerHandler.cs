using LKZ.Server.Network;
using LKZ.Server.Managers;
using LKZ.Network.Common.Events;
namespace LKZ.Server.Handlers.Players
{
    static public class PlayerHandler
    {
        static public void HandlePlayerCreatedMessage(string[] parameters)
        {
            int sendId = Int32.Parse(parameters[0]);

            LobbyManager.AddPlayer(sendId);

            BaseServer.TriggerClientEvent(sendId, "PlayerCreatedMessage", LobbyManager.GetPlayerPosition(sendId));
            BaseServer.TriggerClientEvent(-2, "PlayerJoinedMessage", sendId, LobbyManager.GetPlayerPosition(sendId));
        }

        static public void HandlePlayerMovedMessage(string[] parameters)
        {
            //if (!EventManager.ValidateParameters(parameters, 4))
            //    return;

            int sendId = Int32.Parse(parameters[0]);

            BaseServer.TriggerClientEvent(-2, "PlayerMovedMessage", sendId, parameters[1], parameters[2], parameters[3], parameters[4]);
        }
    }
}