using Buildings.Domain;
using CustomCamera.Domain;
using Map.Domain;
using UnityEngine;

namespace ObjectPooler.Application.Displayers
{
    /**
     * Display buildings within camera visibility
     *
     * Buildings are static (not moveable) so there's no point to run this code if camera is on the same position as in last frame
     *
     * Adding component "BuildingObjectPoolingComponent" to "Building" object inform script that building is already displayed.
     */
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