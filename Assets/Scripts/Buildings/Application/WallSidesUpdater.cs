using System;
using Buildings.Domain.Walls;
using Map.Application;
using Map.Domain;
using UnityEngine;

namespace Buildings.Application
{
    /**
     * It decides whether to turn on / off a wall sides depending on the other walls it comes into contact with.
     */
    public class WallSidesUpdater
    {
        private MapLayerMatrixManager _mapLayerMatrixManager;
        private MapLayerMatrix _mapLayerMatrixWallsEditor;
        private MapLayerMatrix _mapLayerMatrixWalls;
        
        public 
            WallSidesUpdater(
            MapLayerMatrixManager mapLayerMatrixManager,
            MapLayerMatrix mapLayerMatrixWallsEditor,
            MapLayerMatrix mapLayerMatrixWalls
        )
        {
            _mapLayerMatrixManager = mapLayerMatrixManager;
            _mapLayerMatrixWallsEditor = mapLayerMatrixWallsEditor;
            _mapLayerMatrixWalls = mapLayerMatrixWalls;
        }
        
        public void Update(IWall wall)
        {
            if (wall.WallConfigurator == null)
            {
                return;
            }
            
            byte front = _mapLayerMatrixManager.Get(_mapLayerMatrixWallsEditor, (short) (wall.Position2D.x), (short) (wall.Position2D.y + 1));
            byte left = _mapLayerMatrixManager.Get(_mapLayerMatrixWallsEditor, (short) (wall.Position2D.x - 1), (short) (wall.Position2D.y));
            byte back = _mapLayerMatrixManager.Get(_mapLayerMatrixWallsEditor, (short) (wall.Position2D.x), (short) (wall.Position2D.y - 1));
            byte right = _mapLayerMatrixManager.Get(_mapLayerMatrixWallsEditor, (short) (wall.Position2D.x + 1), (short) (wall.Position2D.y));
            
            front += _mapLayerMatrixManager.Get(_mapLayerMatrixWalls, (short) (wall.Position2D.x), (short) (wall.Position2D.y + 1));
            left += _mapLayerMatrixManager.Get(_mapLayerMatrixWalls, (short) (wall.Position2D.x - 1), (short) (wall.Position2D.y));
            back += _mapLayerMatrixManager.Get(_mapLayerMatrixWalls, (short) (wall.Position2D.x), (short) (wall.Position2D.y - 1));
            right += _mapLayerMatrixManager.Get(_mapLayerMatrixWalls, (short) (wall.Position2D.x + 1), (short) (wall.Position2D.y));
            
            SetActive(wall.WallConfigurator.CurrentFront, front <= 0);
            SetActive(wall.WallConfigurator.CurrentLeft, left <= 0);
            SetActive(wall.WallConfigurator.CurrentBack, back <= 0);
            SetActive(wall.WallConfigurator.CurrentRight, right <= 0);
        }

        public long GetLayersLastChangedDate()
        {
            return Math.Max(_mapLayerMatrixWalls.LastUpdate, _mapLayerMatrixWallsEditor.LastUpdate);
        }
        
        private void SetActive(GameObject go, bool active)
        {
            if (active)
            {
                if (!go.activeSelf)
                {
                    go.SetActive(true);
                }
            }
            else
            {
                if (go.activeSelf)
                {
                    go.SetActive(false);
                }
            }
        }
    }
}