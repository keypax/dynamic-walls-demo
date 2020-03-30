using System;
using Buildings.Domain;
using UnityEngine;

namespace Buildings.Application.Spawners
{
    public abstract class BuildingSpawner
    {
        public abstract IBuilding GetBasicBuilding(Guid guid, Vector3 position, byte rotation);
    }
}