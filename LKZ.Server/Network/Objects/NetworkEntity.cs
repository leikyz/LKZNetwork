using LKZ.Server.Enums;
using System;
using System.Numerics;

namespace LKZ.Server.Network.Objects
{
    public class NetworkEntity
    {
        private uint id;
        private Vector3 position;
        private Vector3 rotation;
        private EntityEnum type;
        public NetworkEntity(uint id, EntityEnum type)
        {
            this.id = id;
            this.type = type;
        }

        public uint Id
        {
            get { return id; }
            private set { id = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public EntityEnum Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
