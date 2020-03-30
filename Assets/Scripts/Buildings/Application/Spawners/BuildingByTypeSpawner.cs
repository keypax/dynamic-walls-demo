using System;
using Buildings.Domain;
using Buildings.Domain.Exceptions;
using UnityEngine;

namespace Buildings.Application.Spawners
{
    public class BuildingByTypeSpawner
    {
        private WallSpawner _wallSpawner;
        private TowerSpawner _towerSpawner;

        public BuildingByTypeSpawner(
            WallSpawner wallSpawner,
            TowerSpawner towerSpawner
        )
        {
            _wallSpawner = wallSpawner;
            _towerSpawner = towerSpawner;
        }

        public void Spawn(BuildingType buildingType, Vector3 position, byte rotation)
        {
            switch (buildingType)
            {
                case BuildingType.Wall:
                    _wallSpawner.Spawn(
                        Guid.NewGuid(),
                        position,
                        rotation
                    );
                    break;
                case BuildingType.Tower:
                    _towerSpawner.Spawn(
                        Guid.NewGuid(),
                        position,
                        rotation
                    );
                    break;
                default:
                    throw new BuildingException("No building spawner: " + buildingType);
            }
        }
    }
}