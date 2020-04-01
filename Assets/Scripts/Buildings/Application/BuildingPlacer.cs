using System;
using System.Collections.Generic;
using System.Linq;
using Buildings.Application.Spawners;
using Buildings.Domain;
using Map.Application;
using Map.Domain;
using ObjectPooler.Application.Displayers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Buildings.Application
{
    /**
     * There's a lot going on here.
     * This is the main class that is responsible for arranging the building on the map.
     * Detailed comments below
     */
    public class BuildingPlacer
    {
        //data about building type (size/disabled rotation etc.)
        public BuildingTypeData BuildingTypeData { get; set; }
        //building instance that we're currently previewing
        public IBuilding BuildingPreview { get; set; }
        
        private BuildingFromSpawnerGetter _buildingFromSpawnerGetter;
        private TerrainHitter _terrainHitter;
        private Camera _cameraComponent;
        private BuildingList _buildingList;
        private BuildingsDisplayer _buildingsDisplayer;
        private BuildingCollisionDetector _buildingCollisionDetector;
        private BuildingByTypeSpawner _buildingByTypeSpawner;

        private MapLayerMatrixManager _mapLayerMatrixManager;
        private MapLayerMatrix _mapLayerMatrixWallsEditor;
        
        private Vector3 _dragStartingPosition;
        private byte _dragDirection = 0;
        private byte _lastRotation;
        
        public BuildingPlacer(
            BuildingFromSpawnerGetter buildingFromSpawnerGetter,
            TerrainHitter terrainHitter,
            Camera cameraComponent,
            BuildingList buildingList,
            BuildingsDisplayer buildingsDisplayer,
            BuildingCollisionDetector buildingCollisionDetector,
            BuildingByTypeSpawner buildingByTypeSpawner,
            MapLayerMatrixManager mapLayerMatrixManager,
            MapLayerMatrix mapLayerMatrixWallsEditor
        )
        {
            _buildingFromSpawnerGetter = buildingFromSpawnerGetter;
            _terrainHitter = terrainHitter;
            _cameraComponent = cameraComponent;
            _buildingList = buildingList;
            _buildingsDisplayer = buildingsDisplayer;
            _buildingCollisionDetector = buildingCollisionDetector;
            _buildingByTypeSpawner = buildingByTypeSpawner;

            _mapLayerMatrixManager = mapLayerMatrixManager;
            _mapLayerMatrixWallsEditor = mapLayerMatrixWallsEditor;
        }

        public void Preview()
        {
            //create new building instance 
            if (BuildingPreview == null && BuildingTypeData != null)
            {
                BuildingPreview = CreateNewObject();
                _buildingList.BuildingsEditorPreview = BuildingPreview;
            }

            if (BuildingPreview == null)
            {
                return;
            }

            UpdatePreviewPosition();

            //star dragging
            if (Input.GetMouseButtonDown(0))
            {
                _dragStartingPosition = GetTerrainHitPoint(BuildingPreview);
            }

            //dragging
            if (Input.GetMouseButton(0))
            {
                Vector3 hitPoint = GetTerrainHitPoint(BuildingPreview);
                Vector3 difference = hitPoint - _dragStartingPosition;

                int length = GetLength(BuildingPreview);
                int height = GetHeight(BuildingPreview);

                //check how many buildings will get in the row
                int buildingsRowsX = GetBuildingsRowX(difference, GetLength(BuildingPreview));
                int buildingsRowsY = GetBuildingsRowY(difference, GetHeight(BuildingPreview));
                
                //we store here which building are already in the editor.
                //that helps us to remove buildings that on the end of code will be not actual anymore
                _buildingList.BuildingsEditorCreatedBefore = new List<IBuilding>(_buildingList.BuildingsEditor);

                UpdateDragDirection(buildingsRowsX, buildingsRowsY);
                
                for (int x = 0; x <= buildingsRowsX; x++)
                {
                    for (int y = 0; y <= buildingsRowsY; y++)
                    {
                        //different placing behaviour for walls. I know... i'm lazy. It can be made a lot better
                        if (BuildingPreview.BuildingType == BuildingType.Wall)
                        {
                            if (_dragDirection == 0)
                            {
                                if (buildingsRowsX < buildingsRowsY)
                                {
                                    if (y != 0 && x != buildingsRowsX)
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (x != 0 && y != buildingsRowsY)
                                    {
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                if (_dragDirection == 1)
                                {
                                    if (y != 0 && x != buildingsRowsX)
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (x != 0 && y != buildingsRowsY)
                                    {
                                        continue;
                                    }
                                }
                            }
                        }

                        float newX = GetPreviewBuildingX(difference, length, x);
                        float newZ = GetPreviewBuildingZ(difference, height, y);
                        
                        //create new preview
                        Vector3 position = new Vector3(
                            newX,
                            _dragStartingPosition.y,
                            newZ
                        );
                        
                        //there is no other building in editor on this position, we can create new 
                        if (!ObjectOnPositionExists(position))
                        {
                            AddObject(position);
                        }
                    }
                }

                //remove difference between start and end of the code. Buildings that are not actual anymore will be removed
                foreach (IBuilding building in _buildingList.BuildingsEditorCreatedBefore)
                {
                    RemoveObject(building);
                }
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                Place();
            }
            
            UpdatePositionsAndRotations();
        }
        
        /**
         * Rotates building. There are only 4 directions (0, 1, 2, 3)
         */
        public void Rotate(bool reverse = false)
        {
            if (BuildingTypeData.DisableRotation)
            {
                BuildingPreview.Rotation = 0;
                return;
            }
            
            if (reverse && BuildingPreview.Rotation == 0)
            {
                BuildingPreview.Rotation = 3;
            }
            else
            {
                if (reverse)
                {
                    BuildingPreview.Rotation = Convert.ToByte(BuildingPreview.Rotation - 1);
                }
                else
                {
                    BuildingPreview.Rotation = Convert.ToByte(BuildingPreview.Rotation + 1);
                }
            }
            
            if (BuildingPreview.Rotation > 3)
            {
                BuildingPreview.Rotation = 0;
            }

            _lastRotation = BuildingPreview.Rotation;

            SetRotationForAll();
        }

        /**
         * Clean the editor
         */
        public void Unset()
        {
            BuildingTypeData = null;

            RemoveObject(BuildingPreview);
            BuildingPreview = null;

            foreach (IBuilding objectIsObjectPoolable in _buildingList.BuildingsEditor.ToList())
            {
                RemoveObject(objectIsObjectPoolable);
            }

            if (_buildingList.BuildingsEditorPreview != null)
            {
                _buildingsDisplayer.ReleaseFromPool(_buildingList.BuildingsEditorPreview);
                _buildingList.BuildingsEditorPreview = null;
            }

            _buildingList.BuildingsEditor.Clear();
            _buildingList.BuildingsEditorCreatedBefore.Clear();
        }
        
        private void AddObject(Vector3 position)
        {
            IBuilding building = CreateNewObject();
            building.Position = position;
            building.Position2D = new Vector2(building.Position.x, building.Position.z);
            
            //it can be done better. This is temporary information about wall position in the editor
            if (BuildingPreview.BuildingType == BuildingType.Wall)
            { 
                _mapLayerMatrixManager.Add(_mapLayerMatrixWallsEditor, (short) building.Position2D.x, (short) building.Position2D.y, 0);
            }
                
            _buildingList.BuildingsEditor.Add(building);
        }

        private void RemoveObject(IBuilding building)
        {
            if (building.BuildingType == BuildingType.Wall)
            {
                _mapLayerMatrixManager.Remove(_mapLayerMatrixWallsEditor, (short) building.Position2D.x,
                    (short) building.Position2D.y, 0);
            }

            _buildingList.BuildingsEditor.Remove(building);
            _buildingsDisplayer.ReleaseFromPool(building);
        }

        /**
         * Spawns buildings from editor to map
         */
        private void Place()
        {
            foreach (IBuilding building in _buildingList.BuildingsEditor.ToList())
            {
                RemoveObject(building);
                
                if (_buildingCollisionDetector.IsColliding(building))
                {
                    continue;
                }

                _buildingByTypeSpawner.Spawn(building.BuildingType, building.Position, building.Rotation);
            }
            
            RemoveObject(_buildingList.BuildingsEditorPreview);
            
            BuildingPreview = CreateNewObject();
            _buildingList.BuildingsEditorPreview = BuildingPreview;
        }
        
        private Vector3 GetTerrainHitPoint(IBuilding building)
        {
            Vector3 hitPoint = _terrainHitter.Hit(_cameraComponent.ScreenPointToRay(Input.mousePosition));

            float length = GetLength(building) / 2f;
            float height = GetHeight(building) / 2f;

            float positionX = 0f; 
            float positionZ = 0f; 
            
            switch (building.Rotation)
            {
                case 0:
                    positionX = hitPoint.x - length;
                    positionZ = hitPoint.z - height;
                    break;
                case 1:
                    positionX = hitPoint.x - length;
                    positionZ = hitPoint.z + height - 1;
                    break;
                case 2:
                    positionX = hitPoint.x + length - 1;
                    positionZ = hitPoint.z + height - 1;
                    break;
                case 3:
                    positionX = hitPoint.x + length - 1;
                    positionZ = hitPoint.z - height;
                    break;
            }

            return new Vector3(
                Mathf.Ceil(positionX / 1),
                hitPoint.y,
                Mathf.Ceil(positionZ / 1)
            );
        }
        
        private int GetLength(IBuilding building)
        {
            int length = BuildingTypeData.Length;
            
            if (building.Rotation == 1 ||
                building.Rotation == 3)
            {
                length = BuildingTypeData.Height;
            }

            return length;
        }

        private int GetHeight(IBuilding building)
        {
            int height = BuildingTypeData.Height;
            
            if (building.Rotation == 1 ||
                building.Rotation == 3)
            {
                height = BuildingTypeData.Length;
            }

            return height;
        }
        
        private int GetBuildingsRowX(Vector3 difference, int length)
        {
            return Mathf.FloorToInt(Mathf.Abs(difference.x) / length);
        }

        private int GetBuildingsRowY(Vector3 difference, int height)
        {
            return Mathf.FloorToInt(Mathf.Abs(difference.z) / height);
        }
        
        
        private float GetPreviewBuildingX(Vector3 difference, int length, int x)
        {
            if (difference.x > 0)
            {
                return _dragStartingPosition.x + length * x;
            }
            
            return _dragStartingPosition.x - length * x;
        }

        private float GetPreviewBuildingZ(Vector3 difference, int height, int y)
        {
            if (difference.z > 0)
            {
                return _dragStartingPosition.z + height * y;
            }
            
            return _dragStartingPosition.z - height * y;
        }

        private IBuilding CreateNewObject()
        {
            IBuilding objectIsObjectPoolable = _buildingFromSpawnerGetter.Get(BuildingTypeData.BuildingType, Vector3.zero, 0);

            if (BuildingTypeData.DisableRotation)
            {
                objectIsObjectPoolable.Rotation = 0;
            }
            else
            {
                objectIsObjectPoolable.Rotation = _lastRotation;
            }

            return objectIsObjectPoolable;
        }

        private void UpdateDragDirection(int buildingsRowsX, int buildingsRowsY)
        {
            if (_dragDirection == 0)
            {
                if (buildingsRowsX > buildingsRowsY + 3)
                {
                    _dragDirection = 1;
                }

                if (buildingsRowsY > buildingsRowsX + 3)
                {
                    _dragDirection = 2;
                }
            }
        }

        private void UpdatePositionsAndRotations()
        {
            foreach (IBuilding building in _buildingList.BuildingsEditor)
            {
                UpdatePositionAndRotation(building);
            }
        }

        private void UpdatePositionAndRotation(IBuilding building)
        {
            if (!building.BuildingConfigurator)
            {
                return;
            }
            
            if (building.BuildingConfigurator.transform.position != building.Position)
            { 
                building.BuildingConfigurator.transform.position = building.Position;
            }
                    
            if ((byte) Mathf.FloorToInt(building.BuildingConfigurator.transform.eulerAngles.y / 90f) != building.Rotation)
            {
                var transform = building.BuildingConfigurator.transform;
                var eulerAngles = transform.eulerAngles;
                        
                eulerAngles = new Vector3(
                    eulerAngles.x,
                    BuildingPreview.Rotation * 90f,
                       eulerAngles.z
                    );
                        
                transform.eulerAngles = eulerAngles;
            }
        }

        private void SetRotationForAll()
        {
            foreach (IBuilding building in _buildingList.BuildingsEditor)
            {
                building.Rotation = BuildingPreview.Rotation;
            }
            
            UpdatePositionsAndRotations();
        }

        private void UpdatePreviewPosition()
        {
            if (BuildingPreview.BuildingType == BuildingType.Wall)
            {
                sbyte value = (sbyte) _mapLayerMatrixManager.Get(_mapLayerMatrixWallsEditor, (short) BuildingPreview.Position2D.x, (short) BuildingPreview.Position2D.y);

                if (value > 0)
                {
                    _mapLayerMatrixManager.Remove(_mapLayerMatrixWallsEditor, (short) BuildingPreview.Position2D.x,
                        (short) BuildingPreview.Position2D.y, 0);
                }
            }


            BuildingPreview.Position = GetTerrainHitPoint(BuildingPreview);
            BuildingPreview.Position2D = new Vector2(BuildingPreview.Position.x, BuildingPreview.Position.z);

            if (BuildingPreview.BuildingType == BuildingType.Wall)
            {
                _mapLayerMatrixManager.Add(_mapLayerMatrixWallsEditor, (short) BuildingPreview.Position2D.x, (short) BuildingPreview.Position2D.y, 0);
            }

            UpdatePositionAndRotation(BuildingPreview);
        }

        private bool ObjectOnPositionExists(Vector3 position)
        {
            foreach (IBuilding poolableObjectAlreadyExists in _buildingList.BuildingsEditor)
            {
                if (poolableObjectAlreadyExists.Position == position)
                {
                    _buildingList.BuildingsEditorCreatedBefore.Remove(poolableObjectAlreadyExists);

                    return true;
                }
            }

            return false;
        }
    }
}