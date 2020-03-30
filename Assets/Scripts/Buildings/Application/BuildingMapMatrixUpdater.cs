using Buildings.Domain;
using Map.Application;
using Map.Domain;
using UnityEngine;

namespace Buildings.Application
{
    public class BuildingMapMatrixUpdater
    {
        private BuildingAreaGetter _buildingAreaGetter;

        private MapLayerMatrixManager _mapLayerMatrixManager;
        private MapLayerMatrix _mapLayerMatrixBuildings;

        public BuildingMapMatrixUpdater(
            BuildingAreaGetter buildingAreaGetter,
            MapLayerMatrixManager mapLayerMatrixManager,
            MapLayerMatrix mapLayerMatrixBuildings 
        )
        {
            _buildingAreaGetter = buildingAreaGetter;

            _mapLayerMatrixManager = mapLayerMatrixManager;
            _mapLayerMatrixBuildings = mapLayerMatrixBuildings;
        }

        public void Update(IBuilding building)
        {
            var objectOnMapArea = _buildingAreaGetter.Get(building);
            for (int x = objectOnMapArea.MinX; x < objectOnMapArea.MaxX; x++)
            {
                for (int y = objectOnMapArea.MinY; y < objectOnMapArea.MaxY; y++)
                {
                    _mapLayerMatrixManager.Add(_mapLayerMatrixBuildings, (short) x, (short) y, 0, 1);
                }
            }
        }
    }
}