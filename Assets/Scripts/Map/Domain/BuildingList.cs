using System.Collections.Generic;
using Buildings.Domain;

namespace Map.Domain
{
    public class BuildingList
    {
        public List<Building> Buildings { get; }

        public BuildingList()
        {
            Buildings = new List<Building>();
        }
    }
}