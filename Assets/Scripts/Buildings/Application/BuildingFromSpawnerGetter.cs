using System;
using Buildings.Application.Spawners;
using Buildings.Domain;
using Buildings.Domain.Exceptions;
using UnityEngine;

namespace Buildings.Application
{
    public class BuildingFromSpawnerGetter
    {
        private WallSpawner _wallSpawner;
        private TowerSpawner _towerSpawner;

        public BuildingFromSpawnerGetter(
            WallSpawner wallSpawner,
            TowerSpawner towerSpawner
        )
        {
            _wallSpawner = wallSpawner;
            _towerSpawner = towerSpawner;
        }

        public IBuilding Get(BuildingType buildingType, Vector3 position, byte rotation)
        {
            switch (buildingType)
            {
                case BuildingType.Wall:
                    return _wallSpawner.GetBasicBuilding(
                        Guid.NewGuid(),
                        position,
                        rotation
                    );
                case BuildingType.Tower:
                    return _towerSpawner.GetBasicBuilding(
                        Guid.NewGuid(),
                        position,
                        rotation
                    );
                default:
                    throw new BuildingException("No building spawner: " + buildingType);
            }
        }
    }
}