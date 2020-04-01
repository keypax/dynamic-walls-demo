using System;
using Buildings.Domain;
using Buildings.Domain.Towers;
using Map.Domain;
using UnityEngine;

namespace Buildings.Application.Spawners
{
    /**
     * Spawns a tower.. what did you think? :D
     */
    public class TowerSpawner : BuildingSpawner
    {
        public TowerSpawner(
            BuildingList buildingList, 
            BuildingMapMatrixUpdater buildingMapMatrixUpdater
        ) : base(buildingList, buildingMapMatrixUpdater)
        {
        }
        
        public override IBuilding GetBasicBuilding(Guid guid, Vector3 position, byte rotation)
        {
            ITower tower = new Tower();
            SetValues(tower, guid, BuildingType.Tower, position, rotation);

            return tower;
        }
        
        public ITower Spawn(
            Guid guid,
            Vector3 position,
            byte rotation
        )
        {
            ITower tower = (ITower) GetBasicBuilding(guid, position, rotation);

            AddToLists(tower);

            return tower;
        }
    }
}