using Buildings.Domain;
using Map.Application;
using Map.Domain;
using UnityEngine;

namespace Buildings.Application
{
    public class BuildingCollisionDetector
    {
        private BuildingAreaGetter _buildingAreaGetter;

        private MapLayerMatrixManager _mapLayerMatrixManager;
        private MapLayerMatrix _mapLayerMatrixBuildings;
        private MapLayerMatrix _mapLayerMatrixTrees;
        private MapLayerMatrix _mapLayerMatrixRocks;
        private MapLayerMatrix _mapLayerMatrixWater;
        
        public BuildingCollisionDetector(
            BuildingAreaGetter buildingAreaGetter,
            MapLayerMatrixManager mapLayerMatrixManager,
            MapLayerMatrix mapLayerMatrixBuildings
        )
        {
            _buildingAreaGetter = buildingAreaGetter;

            _mapLayerMatrixManager = mapLayerMatrixManager;
            _mapLayerMatrixBuildings = mapLayerMatrixBuildings;
        }
        
        public bool IsColliding(IBuilding building)
        {
            var buildingArea = _buildingAreaGetter.Get(building);
            
            for (int x = buildingArea.MinX; x < buildingArea.MaxX; x++)
            {
                for (int y = buildingArea.MinY; y < buildingArea.MaxY; y++)
                {
                    if (_mapLayerMatrixManager.Get(_mapLayerMatrixBuildings, (short) x, (short) y) != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}