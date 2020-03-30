using System;
using Buildings.Domain;
using UnityEngine;

namespace Buildings.Application.Spawners
{
    public class TowerSpawner : BuildingSpawner
    {
        public override IBuilding GetBasicBuilding(Guid guid, Vector3 position, byte rotation)
        {
            throw new NotImplementedException();
        }
    }
}