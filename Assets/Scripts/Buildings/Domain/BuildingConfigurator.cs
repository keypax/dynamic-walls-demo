using UnityEngine;

namespace Buildings.Domain
{
    /**
     * Building configurator. You mark what type it is, what size, whether it can be rotated.
     * Based on the data from this place, other scripts respond accordingly.
     * You must attach this to every building
     */
    public class BuildingConfigurator : MonoBehaviour
    {
        public BuildingType buildingType;
        public byte length;
        public byte height;
        public bool disableRotation;
    }
}