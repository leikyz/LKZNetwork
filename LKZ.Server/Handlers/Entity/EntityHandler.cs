using LKZ.Server.Network;
using LKZ.Server.Managers;
using LKZ.Network.Common.Events;
using LKZ.Server.Network.Objects;
using LKZ.Server.Enums;

namespace LKZ.Server.Handlers.Entity
{
    static public class EntityHandler
    {
        static public void HandleEntityCreatedMessage(BaseClient client,string[] parameters)
        {
            int entityType = int.Parse(parameters[1]);

            NetworkEntity entity = new NetworkEntity(BaseServer.NextEntityId, (EntityEnum)entityType);

            if (entityType == 1) // player
            {
                switch (client.Lobby.ClientsCount)
                {
                    case 1:
                        entity.Position = new System.Numerics.Vector3(160, 27, 143);
                        break;
                    case 2:
                        entity.Position = new System.Numerics.Vector3(160, 27, 140);
                        break;
                    case 3:
                        entity.Position = new System.Numerics.Vector3(160, 27, 148);
                        break;
                    case 4:
                        entity.Position = new System.Numerics.Vector3(160, 27, 153);
                        break;
                    default:
                        entity.Position = new System.Numerics.Vector3(160, 27, 140);
                        break;
                }
                
                client.PlayerId = BaseServer.NextEntityId;
            }
        
            client.Lobby.AddEntity(entity);

            BaseServer.TriggerClientEvent(-1, "EntityCreatedMessage", client.Lobby.LobbyId, BaseServer.NextEntityId,
         entityType, entity.Position.X, entity.Position.Y, entity.Position.Z);
            

            BaseServer.NextEntityId++;
        }
        static public void HandleSynchronizeEntities(BaseClient client, string[] parameters)
        {
            foreach(var entity in client.Lobby.Entities.Where(x => x.Id != client.PlayerId))
            {
                BaseServer.TriggerClientEvent((int)client.Id, "EntityCreatedMessage", client.Lobby.LobbyId, entity.Id,
                    (int)entity.Type, entity.Position.X, entity.Position.Y, entity.Position.Z);

            }
        }
        static public void HandleEntityMovementMessage(BaseClient client, string[] parameters)
        {
            if (!EventManager.ValidateParameters(parameters, 3))
                return;

            uint entityId = uint.Parse(parameters[0]);
            float vertical = float.Parse(parameters[1]);
            bool isRunning = bool.Parse(parameters[2]);

            BaseServer.TriggerClientEvent(-2, "EntityMovementMessage", client.Lobby.LobbyId, client.Id, entityId, vertical, isRunning);
        }

        static public void HandleEntityRotationMessage(BaseClient client, string[] parameters)
        {

            if (!EventManager.ValidateParameters(parameters, 4))
                return;

            BaseServer.TriggerClientEvent(-2, "EntityRotationMessage", client.Lobby.LobbyId, client.Id,
                parameters[0], parameters[1], parameters[2], parameters[3]);
        }
        static public void HandleEntityLastPositionMessage(BaseClient client, string[] parameters)
        {
            if (!EventManager.ValidateParameters(parameters, 4))
                return;

            BaseServer.TriggerClientEvent(-2, "EntityLastPositionMessage", client.Lobby.LobbyId, client.Id,
                parameters[0], parameters[1], parameters[2], parameters[3]);
        }


        //static public void HandleMoveEntityMessage(string[] parameters)
        //{

        //}

        //static public void HandleRotateEntityMessage(string[] parameters)
        //{

        //}

    }
}