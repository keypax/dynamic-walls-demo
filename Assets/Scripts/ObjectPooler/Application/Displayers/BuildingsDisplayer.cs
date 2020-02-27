using Buildings.Domain;
using CustomCamera.Domain;
using Map.Domain;
using UnityEngine;

namespace ObjectPooler.Application.Displayers
{
    public class BuildingsDisplayer : AObjectPoolerDisplayer
    {
        private const string Prefix = "building_";
        
        private BuildingList _buildingList;
        
        public BuildingsDisplayer(ObjectPoolerManager objectPoolerManager, BuildingList buildingList) : base(objectPoolerManager)
        {
            _buildingList = buildingList;
        }

        public override bool IsDynamic()
        {
            return false;
        }

        public override void Display(int minX, int maxX, int minY, int maxY, TerrainPositionsFromCameraBoundaries terrainPositionsFromCameraBoundaries)
        {
            GameObject go;

            foreach (Building building in _buildingList.Buildings)
            {
                if (terrainPositionsFromCameraBoundaries.IsInsidePolygon(building.Position2D))
                {
                    //object is not displayed yet
                    if (!building.BuildingObjectPoolingComponent)
                    {
                        go = _objectPoolerManager.SpawnFromPool(
                            Prefix + building.BuildingType,
                            building.Position,
                            building.Rotation
                        );

                        building.BuildingObjectPoolingComponent = go.GetComponent<BuildingObjectPoolingComponent>();
                    }
                }
                else
                {
                    //hide object
                    if (building.BuildingObjectPoolingComponent)
                    {
                        _objectPoolerManager.ReleaseBackToPool(Prefix + building.BuildingType, building.BuildingObjectPoolingComponent.gameObject);
                        
                        building.BuildingObjectPoolingComponent = null;
                    }
                }
            }
        }
    }
}