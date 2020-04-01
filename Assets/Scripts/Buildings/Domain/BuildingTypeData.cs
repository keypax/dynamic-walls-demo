namespace Buildings.Domain
{
    /**
     * Data for a given type of building. It's size, building of a given type can be rotated etc.
     * I do not want to store this information in IBuilding because it is always the same and there is no point in wasting memory
     */
    public class BuildingTypeData
    {
        public BuildingType BuildingType { get; }
        public byte Length { get; }
        public byte Height { get; }
        
        public bool DisableRotation { get; }
        
        public BuildingTypeData(
            BuildingType buildingType,
            byte length,
            byte height,
            bool disableRotation
        )
        {
            BuildingType = buildingType;
            Length = length;
            Height = height;
            DisableRotation = disableRotation;
        }
    }
}