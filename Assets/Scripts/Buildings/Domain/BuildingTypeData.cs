namespace Buildings.Domain
{
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