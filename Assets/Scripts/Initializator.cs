using Buildings.Application;
using Buildings.Application.Spawners;
using Buildings.Domain;
using CustomCamera.Application;
using Map.Application;
using Map.Domain;
using ObjectPooler.Application;
using ObjectPooler.Application.Displayers;
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
        var mapLayerMatrixBuildingsEditor = new MapLayerMatrix((short) mapWidth, (short) mapHeight);
        var mapLayerMatrixWalls = new MapLayerMatrix((short) mapWidth, (short) mapHeight);

        var terrainHitter = new TerrainHitter();
        cameraManager.Init(terrainHitter);
        var terrainPositionsFromCameraBoundariesGetter = new TerrainPositionsFromCameraBoundariesGetter(terrainHitter, cameraComponent);

        var wallSpawner = new WallSpawner();
        var towerSpawner = new TowerSpawner();
        var buildingFromSpawnerGetter = new BuildingFromSpawnerGetter(wallSpawner, towerSpawner);
        
        //displayers
        var buildingsDisplayer = new BuildingsDisplayer(objectPoolerManager, buildingList);
        objectPoolerDisplayer.Init(terrainPositionsFromCameraBoundariesGetter, buildingsDisplayer);
        
        buildingsTypesList.Init();
        
        var buildingAreaGetter = new BuildingAreaGetter(buildingsTypesList);
        var buildingCollisionDetector = new BuildingCollisionDetector(
            buildingAreaGetter,
            mapLayerMatrixManager,
            mapLayerMatrixBuildings
        );

        var buildingPlacer = new BuildingPlacer(
            buildingFromSpawnerGetter,
            terrainHitter,
            cameraComponent,
            buildingList,
            buildingsDisplayer,
            buildingCollisionDetector,
            mapLayerMatrixManager,
            mapLayerMatrixWalls
        );
        
    }
}