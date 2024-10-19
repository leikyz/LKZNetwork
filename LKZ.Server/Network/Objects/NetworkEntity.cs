using System;
using System.Collections.Generic;

namespace LKZ.Server.Network.Objects
{
    public class NetworkEntity
    {
        public uint Id { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }

        public NetworkEntity(uint id)
        {
            Id = id;
        }
    }
}
