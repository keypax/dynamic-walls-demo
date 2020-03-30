using System;
using Buildings.Domain;
using Buildings.Domain.Walls;
using Map.Application;
using Map.Domain;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Buildings.Application.Spawners
{
    public class WallSpawner : BuildingSpawner
    {
        private WallData _wallData;
        private MapLayerMatrixManager _mapLayerMatrixManager;
        private MapLayerMatrix _mapLayerMatrixWalls;

        public WallSpawner(
            BuildingList buildingList,
            BuildingMapMatrixUpdater buildingMapMatrixUpdater,
            WallData wallData,
            MapLayerMatrixManager mapLayerMatrixManager,
            MapLayerMatrix mapLayerMatrixWalls
        ) : base(buildingList, buildingMapMatrixUpdater)
        {
            _wallData = wallData;
            _mapLayerMatrixManager = mapLayerMatrixManager;
            _mapLayerMatrixWalls = mapLayerMatrixWalls;
        }
        
        public override IBuilding GetBasicBuilding(Guid guid, Vector3 position, byte rotation)
        {
            IWall wall = new Wall();
            SetValues(wall, guid, BuildingType.Wall, position, rotation);

            return wall;
        }
        
        public IWall Spawn(
            Guid guid,
            Vector3 position,
            byte rotation
        )
        {
            IWall wall = (IWall) GetBasicBuilding(guid, position, rotation);

            wall.IndexBase = (byte) Random.Range(0, _wallData.BasesCount);
            wall.IndexFront = GetIndex(position, _wallData.FrontsCount);
            wall.IndexRight = GetIndex(position, _wallData.RightsCount);
            wall.IndexBack = GetIndex(position, _wallData.BacksCount);
            wall.IndexLeft = GetIndex(position, _wallData.LeftsCount);
            
            AddToLists(wall);

            _mapLayerMatrixManager.Add(_mapLayerMatrixWalls, (short) wall.Position2D.x, (short) wall.Position2D.y, 0, 1);

            return wall;
        }
        
        private byte GetIndex(Vector3 position, byte count)
        {
            int x = Mathf.CeilToInt(position.x);
            int y = Mathf.CeilToInt(position.z);

            if (y % 2 == 0)
            {
                if (x % 2 == 0)
                {
                    return 1;
                }
            }
            else
            {
                if ((x+1) % 2 == 0)
                {
                    return 1;
                }
            }

            byte index = (byte) Random.Range(0, count);
                
            if (index == 1)
            {
                return 0;
            }

            return index;
        }
    }
}