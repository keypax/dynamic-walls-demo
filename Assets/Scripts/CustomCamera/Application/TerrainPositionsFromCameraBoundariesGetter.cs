using CustomCamera.Domain;
using Map.Application;
using UnityEngine;

namespace CustomCamera.Application
{
    /**
     * Shoots Rays into terrain and get Ray-Terrain contact position.
     * Rays are shot from camera corners minus margin (this prevents sudden object disappear or show)
     */
    public class TerrainPositionsFromCameraBoundariesGetter
    {
        private TerrainHitter _terrainHitter;
        private Camera _cameraComponent;

        public TerrainPositionsFromCameraBoundariesGetter(TerrainHitter terrainHitter, Camera cameraComponent)
        {
            _terrainHitter = terrainHitter;
            _cameraComponent = cameraComponent;
        }
        
        public TerrainPositionsFromCameraBoundaries Get(int margin = 0)
        {
            TerrainPositionsFromCameraBoundaries terrainPositions = new TerrainPositionsFromCameraBoundaries(
                _terrainHitter.Hit(_cameraComponent.ScreenPointToRay(new Vector2(0 - margin, 0 - margin))),
                _terrainHitter.Hit(_cameraComponent.ScreenPointToRay(new Vector2(Screen.width + margin, 0 - margin))),
                _terrainHitter.Hit(_cameraComponent.ScreenPointToRay(new Vector2(0 - margin, Screen.height + margin))),
                _terrainHitter.Hit(_cameraComponent.ScreenPointToRay(new Vector2(Screen.width + margin, Screen.height + margin)))
            );
            
            return terrainPositions;
        }
    }
}