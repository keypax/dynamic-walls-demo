using System;
using UnityEngine;

namespace Buildings.Domain
{
    /**
     * Main parent for all the buildings. It contains all common data which evey building has.
     */
    public interface IBuilding
    {
        Guid Guid { get; set; }
        
        Vector3 Position { get; set; }
        Vector2 Position2D { get; set; }
        byte Rotation { get; set; }
        
        BuildingType BuildingType { get; set; }
        
        BuildingConfigurator BuildingConfigurator { get; set; }
    }
}