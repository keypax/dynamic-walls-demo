using Buildings.Application;
using Buildings.Application.Spawners;
using Buildings.Domain;
using Buildings.Domain.Walls;
using CustomCamera.Application;
using Map.Application;
using Map.Domain;
using ObjectPooler.Application;
using ObjectPooler.Application.Displayers;
using UI.Application;
using UnityEngine;
using CameraManager = CustomCamera.Application.CameraManager;

/**
 * This class is needed to make code in Dependency Injection style.
 * https://www.youtube.com/watch?v=IKD2-MAkXyQ
 *
 * This is the first place called by Unity to our scripts and here we create all classes needed in other places.
 * Here are also the references to existings components attached to GameObjects.
 */
public class Initializator : MonoBehaviour
{
    public Terrain terrain;
    
    public ObjectPoolerDisplayer objectPoolerDisplayer;
    public ObjectPoolerManager objectPoolerManager;
    
    public Camera cameraComponent;
    public CameraManager cameraManager;

    public BuildingsTypesList buildingsTypesList;
    public Canvas canvas;
    public ViewManager viewManager;
    public WallData wallData;

    public void Awake()
    {
        //config
        var terrainData = terrain.terrainData;
        
        int mapWidth = terrainData.heightmapResolution;
        int mapHeight = terrainData.heightmapResolution;
        
        //lists
        var buildingList = new BuildingList();
        
        //matrices
        var mapLayerMatrixManager = new MapLayerMatrixManager();
        
        var mapLayerMatrixBuildings = new MapLayerMatrix((short) mapWidth, (short) mapHeight);
        var mapLayerMatrixWalls = new MapLayerMatrix((short) mapWidth, (short) mapHeight);
        var mapLayerMatrixWallsEditor = new MapLayerMatrix((short) mapWidth, (short) mapHeight);

        var terrainHitter = new TerrainHitter();
        cameraManager.Init(terrainHitter);
        var terrainPositionsFromCameraBoundariesGetter = new TerrainPositionsFromCameraBoundariesGetter(terrainHitter, cameraComponent);

        buildingsTypesList.Init();
        
        var buildingAreaGetter = new BuildingAreaGetter(buildingsTypesList);
        var buildingMapMatrixUpdater = new BuildingMapMatrixUpdater(
            buildingAreaGetter,
            mapLayerMatrixManager,
            mapLayerMatrixBuildings
        );
        
        var wallSpawner = new WallSpawner(
            buildingList, 
            buildingMapMatrixUpdater,
            wallData, 
            mapLayerMatrixManager, 
            mapLayerMatrixWalls
        );
        
        var towerSpawner = new TowerSpawner(buildingList, buildingMapMatrixUpdater);
        var buildingFromSpawnerGetter = new BuildingFromSpawnerGetter(wallSpawner, towerSpawner);
        
        var wallSidesUpdater = new WallSidesUpdater(
            mapLayerMatrixManager, 
            mapLayerMatrixWallsEditor,
            mapLayerMatrixWalls
        );
        
        //displayers
        var buildingsDisplayer = new BuildingsDisplayer(objectPoolerManager, buildingList, wallSidesUpdater);
        objectPoolerDisplayer.Init(terrainPositionsFromCameraBoundariesGetter, buildingsDisplayer);
        
        var buildingCollisionDetector = new BuildingCollisionDetector(
            buildingAreaGetter,
            mapLayerMatrixManager,
            mapLayerMatrixBuildings
        );
        
        var buildingByTypeSpawner = new BuildingByTypeSpawner(wallSpawner, towerSpawner);

        var buildingPlacer = new BuildingPlacer(
            buildingFromSpawnerGetter,
            terrainHitter,
            cameraComponent,
            buildingList,
            buildingsDisplayer,
            buildingCollisionDetector,
            buildingByTypeSpawner,
            mapLayerMatrixManager,
            mapLayerMatrixWallsEditor
        );
        
        viewManager.Init(buildingsTypesList, buildingPlacer, canvas);
    }
}