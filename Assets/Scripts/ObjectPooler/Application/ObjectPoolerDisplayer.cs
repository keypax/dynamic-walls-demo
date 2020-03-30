using System.Collections.Generic;
using CustomCamera.Application;
using ObjectPooler.Application.Displayers;
using UnityEngine;
using UnityEngine.Profiling;

namespace ObjectPooler.Application
{
    /**
     * This class runs other Displayers (BuldingDisplayer & PeopleDisplayer)
     * Thanks to this solution we only send Rays once per frame.
     * There is also simple cache, that do not run non-dynamic Displayer if camera stands still.
     */
    public class ObjectPoolerDisplayer : MonoBehaviour
    {
        public int margin = 50;
        
        private TerrainPositionsFromCameraBoundariesGetter _terrainPositionsFromCameraBoundariesGetter;
        
        private List<AObjectPoolerDisplayer> _displayers = new List<AObjectPoolerDisplayer>();
        
        private string _lastPositions;

        public void Init(
            TerrainPositionsFromCameraBoundariesGetter terrainPositionsFromCameraBoundariesGetter,
            BuildingsDisplayer buildingsDisplayer
        )
        {
            _terrainPositionsFromCameraBoundariesGetter = terrainPositionsFromCameraBoundariesGetter;

            _displayers.Add(buildingsDisplayer);
        }

        public void Update()
        {
            var terrainPositions = _terrainPositionsFromCameraBoundariesGetter.Get(margin);

            int minX = Mathf.FloorToInt(terrainPositions.GetMinX());
            int maxX = Mathf.CeilToInt(terrainPositions.GetMaxX());
            int minY = Mathf.FloorToInt(terrainPositions.GetMinY());
            int maxY = Mathf.CeilToInt(terrainPositions.GetMaxY());
            
            //simple cache
            string currentPositions = minX.ToString() + maxX.ToString() + minY.ToString() + maxY.ToString();

            foreach (AObjectPoolerDisplayer displayer in _displayers)
            {
                //simple cache for static displayers
                if (_lastPositions == currentPositions && displayer.IsDynamic() == false)
                {
                    continue;
                }

                Profiler.BeginSample("ObjectPoolerDisplayer_" + displayer.GetType().Name);
                displayer.Display(minX, maxX, minY, maxY, terrainPositions);
                Profiler.EndSample();
            }

            _lastPositions = currentPositions;
        }
    }
}