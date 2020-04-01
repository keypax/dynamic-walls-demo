using System;
using Buildings.Domain;
using Map.Domain;
using UnityEngine;

namespace Buildings.Application.Spawners
{
    /**
     * Parent for all spawners. It contains all the common code.
     * All buildings need to be:
     *  - added to building list
     *  - added to 2D building matrix
     */
    public abstract class BuildingSpawner
    {
        private BuildingList _buildingList;
        private BuildingMapMatrixUpdater _buildingMapMatrixUpdater;

        protected BuildingSpawner(BuildingList buildingList, BuildingMapMatrixUpdater buildingMapMatrixUpdater)
        {
            _buildingList = buildingList;
            _buildingMapMatrixUpdater = buildingMapMatrixUpdater;
        }
        
        public abstract IBuilding GetBasicBuilding(Guid guid, Vector3 position, byte rotation);
        
        protected void SetValues(
            IBuilding building,
            Guid guid,
            BuildingType buildingType,
            Vector3 position, 
            byte rotation
        )
        {
            building.Guid = guid;
            building.BuildingType = buildingType;
            building.Position = position;
            building.Position2D = new Vector2(position.x, position.z);
            building.Rotation = rotation;
        }
        
        protected void AddToLists(IBuilding building)
        {
            _buildingList.Buildings.Add(building);
            
            _buildingMapMatrixUpdater.Update(building);
        }
    }
}