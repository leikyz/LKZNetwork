//using LKZ.Server.Network;
//using LKZ.Server.Managers;
//using LKZ.Network.Common.Events;
//namespace LKZ.Server.Handlers.Players
//{
//    static public class PlayerHandler
//    {
//        static public void HandlePlayerCreatedMessage(string[] parameters)
//        {
//            //if (!EventManager.ValidateParameters(parameters, 1))
//            //    return;

//            BaseClient client = BaseServer.GetClient(uint.Parse(parameters[0]));
//            client.PlayerId = BaseServer.NextEntityId;
//            BaseServer.NextEntityId++;

//            if (client.Lobby == null)
//            {
//                Lobby lobby = LobbyManager.GetLobby(int.Parse(parameters[1]));
//                lobby.AddClient(client);
//                client.Lobby = lobby;
              
//            }

//            BaseServer.TriggerClientEvent(int.Parse(parameters[0]), "PlayerCreatedMessage", client.PlayerId, client.Lobby.ClientsCount);
//        }

//        static public void HandlePlayerMoveMessage(string[] parameters)
//        {
//            //if (!EventManager.ValidateParameters(parameters, 5))
//            //    return;

//            //int sendId = Int32.Parse(parameters[0]);

//            //BaseServer.TriggerClientEvent(-2, "PlayerMoveMessage", sendId, parameters[1], parameters[2], parameters[3], parameters[4]);
//        }

//        static public void HandlePlayerRotationMessage(string[] parameters)
//        {
//            //if (!EventManager.ValidateParameters(parameters, 3))
//            //    return;

//            //int sendId = Int32.Parse(parameters[0]);

//            //BaseServer.TriggerClientEvent(-2, "PlayerRotationMessage", sendId, parameters[1], parameters[2]);
//        }
//    }
//}