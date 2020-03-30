using Buildings.Application;
using Buildings.Domain;
using UnityEngine;

namespace UI.Application
{
    public class ViewManager : MonoBehaviour
    {
        private BuildingsTypesList _buildingsTypesList;
        private BuildingPlacer _buildingPlacer;
        private Canvas _canvas;
        private bool _buildMode;

        public void Init(
            BuildingsTypesList buildingsTypesList,
            BuildingPlacer buildingPlacer,
            Canvas canvas
        )
        {
            _buildingsTypesList = buildingsTypesList;
            _buildingPlacer = buildingPlacer;
            _canvas = canvas;
        }

        public void Update()
        {
            if (!_buildMode)
            {
                return;
            }

            _buildingPlacer.Preview();
            _canvas.gameObject.SetActive(false);

            if (Input.GetMouseButtonDown(0))
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    UnsetBuildingMode();
                }
            }
                
            if (Input.GetKeyDown(KeyCode.R))
            {
                _buildingPlacer.Rotate();
            }

            if (Input.mouseScrollDelta.y > 0)
            { 
                _buildingPlacer.Rotate();
            }
                
            if (Input.mouseScrollDelta.y < 0)
            {
                _buildingPlacer.Rotate(true);
            }

            if (Input.GetMouseButtonDown(1))
            {
                UnsetBuildingMode();
            }
                
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UnsetBuildingMode();
            }
        }

        public void SetBuldingMode(BuildingType buildingType)
        {
            _buildMode = true;
            _buildingPlacer.BuildingTypeData = _buildingsTypesList.GetByType(buildingType);
            _buildingPlacer.BuildingPreview = null;
        }

        private void UnsetBuildingMode()
        {
            _buildMode = false;
            _canvas.gameObject.SetActive(true);
            _buildingPlacer.Unset();
        }
    }
}