using System;
using Buildings.Domain;
using Map.Domain;
using Noise.Application;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map.Application
{
    public class MapGenerator
    {
        private TerrainData _terrainData;
        private NoiseGenerator _noiseGenerator;
        private BuildingList _buildingList;

        public MapGenerator(
            TerrainData terrainData,
            NoiseGenerator noiseGenerator,
            BuildingList buildingList
        )
        {
            _terrainData = terrainData;
            _noiseGenerator = noiseGenerator;
            _buildingList = buildingList;
        }
        
        public void Generate()
        {
            //get all possible values in BuildingType enumerator
            BuildingType[] buildingTypes = (BuildingType[]) Enum.GetValues(typeof(BuildingType));
            
            for (int x = 0; x < _terrainData.size.x; x++)
            {
                for (int y = 0; y < _terrainData.size.z; y++)
                {
                    if (_noiseGenerator.Generate(x, y, 320) > 0.7f)
                    {
                        Building building = new Building();
                        building.Guid = Guid.NewGuid();
                        building.Position = new Vector3(x, _terrainData.GetHeight(x, y), y);
                        building.Position2D = new Vector2(x, y);
                        building.Rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                        
                        //randomization started from 1 because enum on value 0 is: BuildingType.None
                        building.BuildingType = buildingTypes[Random.Range(1, buildingTypes.Length)];
                        
                        _buildingList.Buildings.Add(building);
                    }
                }
            }
        }
    }
}