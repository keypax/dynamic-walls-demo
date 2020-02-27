using System.Collections.Generic;
using Buildings.Domain;

namespace Map.Domain
{
    /**
     * List of all buildings on the map
     */
    public class BuildingList
    {
        public List<Building> Buildings { get; }

        public BuildingList()
        {
            Buildings = new List<Building>();
        }
    }
}