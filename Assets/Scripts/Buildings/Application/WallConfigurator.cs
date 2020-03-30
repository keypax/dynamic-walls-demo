using Buildings.Domain.Walls;
using UnityEngine;

namespace Buildings.Application
{
    public class WallConfigurator : MonoBehaviour
    {
        public GameObject[] bases;
        public GameObject[] fronts;
        public GameObject[] rights;
        public GameObject[] backs;
        public GameObject[] lefts;

        public GameObject[] disableOnStart;
        
        public GameObject CurrentBase { get; set; }
        public GameObject CurrentFront { get; set; }
        public GameObject CurrentRight { get; set; }
        public GameObject CurrentBack { get; set; }
        public GameObject CurrentLeft { get; set; }

        private byte _currentIndexBase;
        private byte _currentIndexFront;
        private byte _currentIndexRight;
        private byte _currentIndexBack;
        private byte _currentIndexLeft;
        
        private IWall _wall;
        
        public void SetWall(IWall wall)
        {
            foreach (GameObject go in disableOnStart)
            {
                go.SetActive(false);
            }
            
            DisableIncorrectSides(wall);
            
            UpdateCurrentIndexes(wall);

            EnableSides();
        }
        
        private void DisableIncorrectSides(IWall wall)
        {
            if (_currentIndexBase != wall.IndexBase)
            {
                if (CurrentBase)
                {
                    CurrentBase.SetActive(false);
                }
            }
            
            if (_currentIndexFront != wall.IndexFront)
            {
                if (CurrentFront)
                {
                    CurrentFront.SetActive(false);
                }
            }
            
            if (_currentIndexRight != wall.IndexRight)
            {
                if (CurrentRight)
                {
                    CurrentRight.SetActive(false);
                }
            }
            
            if (_currentIndexBack != wall.IndexBack)
            {
                if (CurrentBack)
                {
                    CurrentBack.SetActive(false);
                }
            }
            
            if (_currentIndexLeft != wall.IndexLeft)
            {
                if (CurrentLeft)
                {
                    CurrentLeft.SetActive(false);
                }
            }
        }

        private void UpdateCurrentIndexes(IWall wall)
        {
            _currentIndexBase = wall.IndexBase;
            _currentIndexFront = wall.IndexFront;
            _currentIndexRight = wall.IndexRight;
            _currentIndexBack = wall.IndexBack;
            _currentIndexLeft = wall.IndexLeft;
            
            CurrentBase = bases[wall.IndexBase];
            CurrentFront = fronts[wall.IndexFront];
            CurrentRight = rights[wall.IndexRight];
            CurrentBack = backs[wall.IndexBack];
            CurrentLeft = lefts[wall.IndexLeft];
        }

        private void EnableSides()
        {
            CurrentBase.SetActive(true);
            CurrentFront.SetActive(true);
            CurrentRight.SetActive(true);
            CurrentBack.SetActive(true);
            CurrentLeft.SetActive(true);
        }
    }
}