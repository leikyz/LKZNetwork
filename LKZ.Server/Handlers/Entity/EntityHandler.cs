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

            if (entityType == 1) // player   
                client.PlayerId = BaseServer.NextEntityId;
            client.Lobby.AddEntity(new NetworkEntity(BaseServer.NextEntityId, (EntityEnum)entityType));
                  
            BaseServer.TriggerClientEvent(-1, "EntityCreatedMessage", client.Lobby.LobbyId, BaseServer.NextEntityId, entityType);

            BaseServer.NextEntityId++;
        }
        static public void HandleSynchronizeEntities(BaseClient client, string[] parameters)
        {

            foreach(var entity in client.Lobby.Entities)
            {
                BaseServer.TriggerClientEvent((int)client.Id, "EntityCreatedMessage", client.Lobby.LobbyId, BaseServer.NextEntityId, (int)entity.Type);

            }
        }
        static public void HandleEntityPositionMessage(string[] parameters)
        {

        }

        //static public void HandleEntityRotationMessage(string[] parameters)
        //{

        //}

        //static public void HandleMoveEntityMessage(string[] parameters)
        //{

        //}

        //static public void HandleRotateEntityMessage(string[] parameters)
        //{

        //}

    }
}