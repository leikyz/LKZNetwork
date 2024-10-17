using LKZ.Server.Network;
using LKZ.Server.Managers;
using LKZ.Network.Common.Events;
namespace LKZ.Server.Handlers.Players
{
    static public class PlayerHandler
    {
        static public void HandlePlayerCreatedMessage(string[] parameters)
        {
            if (!EventManager.ValidateParameters(parameters, 1))
                return;

            int sendId = Int32.Parse(parameters[0]);

            LobbyManager.AddPlayer(sendId);

            BaseServer.TriggerClientEvent(sendId, "PlayerCreatedMessage", LobbyManager.GetPlayerPosition(sendId));
            BaseServer.TriggerClientEvent(-2, "PlayerJoinedMessage", sendId, LobbyManager.GetPlayerPosition(sendId));
        }

        static public void HandlePlayerMoveMessage(string[] parameters)
        {
            if (!EventManager.ValidateParameters(parameters, 5))
                return;

            int sendId = Int32.Parse(parameters[0]);

            BaseServer.TriggerClientEvent(-2, "PlayerMoveMessage", sendId, parameters[1], parameters[2], parameters[3], parameters[4]);
        }

        static public void HandlePlayerRotationMessage(string[] parameters)
        {
            if (!EventManager.ValidateParameters(parameters, 3))
                return;

            int sendId = Int32.Parse(parameters[0]);

            BaseServer.TriggerClientEvent(-2, "PlayerRotationMessage", sendId, parameters[1], parameters[2]);
        }
    }
}