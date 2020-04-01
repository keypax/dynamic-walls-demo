using System;
using System.Collections.Generic;
using Buildings.Domain.Exceptions;
using UnityEngine;

namespace Buildings.Domain
{
    /**
     * List of all building types.
     * It's the "prefabs" array we throw in every building. The script automatically extracts all necessary data and stores it in "BuildingTypeData".
     * When we need data for a given type of building, we extract it using "GetByType"
     */
    public class BuildingsTypesList : MonoBehaviour
    {
        public GameObject[] prefabs;

        private Dictionary<BuildingType, BuildingTypeData> _buildingTypesData = new Dictionary<BuildingType, BuildingTypeData>();

        public void Init()
        {
            foreach (GameObject go in prefabs)
            {
                BuildingConfigurator buildingConfigurator = go.GetComponent<BuildingConfigurator>();
                
                try
                {
                    BuildingTypeData buildingTypeData = new BuildingTypeData(
                        buildingConfigurator.buildingType,
                        buildingConfigurator.length,
                        buildingConfigurator.height,
                        buildingConfigurator.disableRotation
                    );
                    
                    _buildingTypesData.Add(buildingConfigurator.buildingType, buildingTypeData);
                }
                catch (Exception e)
                {
                    Debug.LogError("GameObject: " + go.name + ". " + e);
                    throw;
                }
            }
        }

        public BuildingTypeData GetByType(BuildingType buildingType)
        {
            try
            {
                return _buildingTypesData[buildingType];
            }
            catch (Exception)
            {
                throw new BuildingTypeNotSetInPrefabException(buildingType.ToString());
            }
        }
    }
}