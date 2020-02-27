using CustomCamera.Application;
using Map.Application;
using Map.Domain;
using Noise.Application;
using ObjectPooler.Application;
using ObjectPooler.Application.Displayers;
using UnityEngine;

public class Initializator : MonoBehaviour
{
    public Terrain terrain;
    
    public ObjectPoolerDisplayer objectPoolerDisplayer;
    public ObjectPoolerManager objectPoolerManager;
    
    public Camera cameraComponent;
    
    public void Awake()
    {
        var noiseGenerator = new NoiseGenerator(
            Mathf.RoundToInt(terrain.terrainData.size.x), 
            Mathf.RoundToInt(terrain.terrainData.size.z)
        );

        var buildingList = new BuildingList();
        
        var mapGenerator = new MapGenerator(terrain.terrainData, noiseGenerator, buildingList);
        
        mapGenerator.Generate();
        
        var terrainHitter = new TerrainHitter();

        var terrainPositionsFromCameraBoundariesGetter = new TerrainPositionsFromCameraBoundariesGetter(terrainHitter, cameraComponent);
        
        var buildingsDisplayer = new BuildingsDisplayer(objectPoolerManager, buildingList);
        
        objectPoolerDisplayer.Init(terrainPositionsFromCameraBoundariesGetter, buildingsDisplayer);
    }
}