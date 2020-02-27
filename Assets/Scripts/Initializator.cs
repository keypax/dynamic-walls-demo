using CustomCamera.Application;
using Map.Application;
using Map.Domain;
using Noise.Application;
using ObjectPooler.Application;
using ObjectPooler.Application.Displayers;
using People.Application;
using UnityEngine;
using CameraManager = CustomCamera.Application.CameraManager;

public class Initializator : MonoBehaviour
{
    public Terrain terrain;
    
    public ObjectPoolerDisplayer objectPoolerDisplayer;
    public ObjectPoolerManager objectPoolerManager;
    
    public Camera cameraComponent;
    public CameraManager cameraManager;

    public PeopleManager peopleManager;
    
    public void Awake()
    {
        var noiseGenerator = new NoiseGenerator(
            Mathf.RoundToInt(terrain.terrainData.size.x), 
            Mathf.RoundToInt(terrain.terrainData.size.z)
        );

        //lists
        var buildingList = new BuildingList();
        var personList = new PersonList();
        
        peopleManager.Init(personList);
        
        var mapGenerator = new MapGenerator(terrain.terrainData, noiseGenerator, buildingList, personList);
        mapGenerator.Generate();
        
        var terrainHitter = new TerrainHitter();
        cameraManager.Init(terrainHitter);
        var terrainPositionsFromCameraBoundariesGetter = new TerrainPositionsFromCameraBoundariesGetter(terrainHitter, cameraComponent);
        
        //displayers
        var buildingsDisplayer = new BuildingsDisplayer(objectPoolerManager, buildingList);
        var peopleDisplayer = new PeopleDisplayer(objectPoolerManager, personList);
        
        objectPoolerDisplayer.Init(terrainPositionsFromCameraBoundariesGetter, buildingsDisplayer, peopleDisplayer);
    }
}