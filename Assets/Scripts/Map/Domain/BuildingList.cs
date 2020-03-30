using System.Collections.Generic;
using Buildings.Domain;

namespace Map.Domain
{
    /**
     * List of all buildings on the map
     */
    public class BuildingList
    {
        public List<IBuilding> Buildings { get; }
        public List<IBuilding> BuildingsEditor { get; }
        public List<IBuilding> BuildingsEditorCreatedBefore { get; set; }
        public IBuilding BuildingsEditorPreview { get; set; }

        public BuildingList()
        {
            Buildings = new List<IBuilding>();
            BuildingsEditor = new List<IBuilding>();
            BuildingsEditorCreatedBefore = new List<IBuilding>();
        }
    }
}