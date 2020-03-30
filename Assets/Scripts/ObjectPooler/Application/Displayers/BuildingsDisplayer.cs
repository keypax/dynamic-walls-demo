using Buildings.Application;
using Buildings.Domain;
using Buildings.Domain.Walls;
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
        private WallSidesUpdater _wallSidesUpdater;
        
        public BuildingsDisplayer(
            ObjectPoolerManager objectPoolerManager,
            BuildingList buildingList,
            WallSidesUpdater wallSidesUpdater
        ) : base(objectPoolerManager)
        {
            _buildingList = buildingList;
            _wallSidesUpdater = wallSidesUpdater;
        }

        public override bool IsDynamic()
        {
            return false;
        }

        public override void Display(int minX, int maxX, int minY, int maxY, TerrainPositionsFromCameraBoundaries terrainPositionsFromCameraBoundaries)
        {
            if (_buildingList.BuildingsEditorPreview != null)
            {
                DisplayOne(_buildingList.BuildingsEditorPreview, terrainPositionsFromCameraBoundaries);
            }

            foreach (IBuilding building in _buildingList.Buildings)
            {
               DisplayOne(building, terrainPositionsFromCameraBoundaries);
            }
            
            foreach (IBuilding building in _buildingList.BuildingsEditor)
            {
                DisplayOne(building, terrainPositionsFromCameraBoundaries);
            }
        }
        
        public void ReleaseFromPool(IBuilding building)
        {
            if (building.BuildingConfigurator)
            {
                _objectPoolerManager.ReleaseBackToPool("building_" + building.BuildingType, building.BuildingConfigurator.gameObject);
                building.BuildingConfigurator = null;
            }
            
            /*if (building is IWall wall)
            {
                wall.WallConfigurator = null;
            }*/
        }

        private void DisplayOne(IBuilding building, TerrainPositionsFromCameraBoundaries terrainPositions)
        {
            GameObject go;
            
            if (terrainPositions.IsInsidePolygon(building.Position2D))
            {
                //object is not displayed yet
                if (!building.BuildingConfigurator)
                { 
                    go = _objectPoolerManager.SpawnFromPool(
                        Prefix + building.BuildingType,
                        building.Position,
                        Quaternion.Euler(0, building.Rotation * 90f, 0)
                    );

                    building.BuildingConfigurator = go.GetComponent<BuildingConfigurator>();
                    
                    if (building is IWall wall)
                    {
                        wall.WallConfigurator = go.GetComponent<WallConfigurator>();
                        wall.WallConfigurator.SetWall(wall);
                        _wallSidesUpdater.Update(wall);
                    }
                }
                else
                {
                    if (building is IWall wall)
                    {
                        //if (_mapLayersLastUpdate != _wallSidesUpdater.GetLayersLastChangedDate())
                        {
                            _wallSidesUpdater.Update(wall);
                        }
                    }
                }
            }
            else
            {
                //hide object
                if (building.BuildingConfigurator)
                { 
                    _objectPoolerManager.ReleaseBackToPool(Prefix + building.BuildingType, building.BuildingConfigurator.gameObject); 
                    building.BuildingConfigurator = null;
                }
            }
        }
    }
}