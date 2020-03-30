namespace Buildings.Domain.Exceptions
{
    public class BuildingTypeNotSetInPrefabException : BuildingException
    {
        public BuildingTypeNotSetInPrefabException(string message = null) : base(message) {}
    }
}