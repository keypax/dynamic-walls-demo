using System;
using UnityEngine;

namespace Buildings.Domain.Walls
{
    public class Wall : IWall
    {
        public Guid Guid { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Position2D { get; set; }
        public byte Rotation { get; set; }
        public BuildingType BuildingType { get; set; }
        public BuildingConfigurator BuildingConfigurator { get; set; }
    }
}