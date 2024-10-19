using LKZ.Server.Network.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.Server.Managers
{
    public class EntityManager
    {
        private List<NetworkEntity> entities;

        public EntityManager()
        {
            entities = new List<NetworkEntity>();
        }

        public void AddEntity(NetworkEntity entity)
        {
            if (!entities.Contains(entity))
            {
                entities.Add(entity);
            }
        }

        public void RemoveEntity(NetworkEntity entity)
        {
            if (entities.Contains(entity))
            {
                entities.Remove(entity);
            }
        }

        public List<NetworkEntity> GetAllEntities()
        {
            return new List<NetworkEntity>(entities);
        }
    }
}
