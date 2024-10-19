using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKZ.Server.Network.Objects
{
    public class Lobby
    {
        public int LobbyId { get; private set; }
        private List<BaseClient> clients;
        private List<NetworkEntity> entities;
        private int spawnIndex;
        public Lobby(int id)
        {
            LobbyId = id;
            clients = new List<BaseClient>();
            entities = new List<NetworkEntity>();
            spawnIndex = 0;
        }

        public List<NetworkEntity> Entities => entities;    

        public void AddClient(BaseClient client)
        {
            if (!clients.Contains(client))
            {
                clients.Add(client);
            }
        }

        public void AddEntity(NetworkEntity entity)
        {
            if (!entities.Contains(entity))
            {
                entities.Add(entity);
            }
        }

        public void RemovePlayer(BaseClient client)
        {
            if (clients.Contains(client))
            {
                clients.Remove(client);
            }
        }

        public List<BaseClient> Clients => clients;

        public int ClientsCount => clients.Count;

        public int SpawnIndex
        {
            get { return spawnIndex; }
            set { spawnIndex = value; }
        }
    }

}
