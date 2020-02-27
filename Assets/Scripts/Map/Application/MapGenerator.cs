using System;
using Buildings.Domain;
using Map.Domain;
using Noise.Application;
using People.Domain;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map.Application
{
    public class MapGenerator
    {
        private TerrainData _terrainData;
        private NoiseGenerator _noiseGenerator;
        private BuildingList _buildingList;
        private PersonList _personList;

        public MapGenerator(
            TerrainData terrainData,
            NoiseGenerator noiseGenerator,
            BuildingList buildingList,
            PersonList personList
        )
        {
            _terrainData = terrainData;
            _noiseGenerator = noiseGenerator;
            _buildingList = buildingList;
            _personList = personList;
        }
        
        public void Generate()
        {
            //get all possible values in BuildingType enumerator
            BuildingType[] buildingTypes = (BuildingType[]) Enum.GetValues(typeof(BuildingType));
            PersonType[] personTypes = (PersonType[]) Enum.GetValues(typeof(PersonType));
            
            for (int x = 0; x < _terrainData.size.x; x = x + 2)
            {
                for (int y = 0; y < _terrainData.size.z; y = y + 2)
                {
                    float noise = _noiseGenerator.Generate(x, y, 320);
                    
                    if (noise > 0.7f)
                    {
                        Building building = new Building();
                        building.Guid = Guid.NewGuid();
                        building.Position = new Vector3(x, _terrainData.GetHeight(x, y), y);
                        building.Position2D = new Vector2(x, y);
                        building.Rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        
                        building.BuildingType = buildingTypes[Random.Range(0, buildingTypes.Length)];
                        
                        _buildingList.Buildings.Add(building);
                    }
                    else if (noise < 0.2f)
                    {
                        Person person = new Person();
                        person.Guid = Guid.NewGuid();
                        person.Position = new Vector3(x, _terrainData.GetHeight(x, y), y);
                        person.Position2D = new Vector2(x, y);
                        person.Rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        person.PersonMode = PersonMode.Idle;
                        person.PersonType = personTypes[Random.Range(0, personTypes.Length)];
                        
                        _personList.People.Add(person);
                    }
                }
            }

            Debug.LogFormat("There are {0} buildings on the map", _buildingList.Buildings.Count);
            Debug.LogFormat("There are {0} people on the map", _personList.People.Count);
        }
    }
}