using System;
using UnityEngine;

namespace Buildings.Domain
{
    public class Building
    {
        public Guid Guid { get; set; }
        public Vector3 Position { get; set; }
        public Vector2 Position2D { get; set; }
        public Quaternion Rotation { get; set; }
        public BuildingType BuildingType { get; set; }
        
        public BuildingObjectPoolingComponent BuildingObjectPoolingComponent { get; set; }
    }
}