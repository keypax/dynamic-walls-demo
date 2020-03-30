using Buildings.Domain;
using UnityEngine;

namespace UI.Application
{
    public class BuildingChooser : MonoBehaviour
    {
        public ViewManager viewManager;
        public BuildingType buildingType;

        public void Choose()
        {
            viewManager.SetBuldingMode(buildingType);
        }
    }
}