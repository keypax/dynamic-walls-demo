using System;
using Buildings.Application;
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
        
        public WallConfigurator WallConfigurator { get; set; }
        public byte IndexBase { get; set; }
        public byte IndexFront { get; set; }
        public byte IndexRight { get; set; }
        public byte IndexBack { get; set; }
        public byte IndexLeft { get; set; }
    }
}