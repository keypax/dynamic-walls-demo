using CustomCamera.Domain;
using ObjectPooler.Domain;

namespace ObjectPooler.Application.Displayers
{
    /**
     * All classes with responsibility to display some objects (Displayers) should extends this abstract class
     */
    public abstract class AObjectPoolerDisplayer : IObjectPoolerDisplayer
    {
        public abstract bool IsDynamic();
        public abstract void Display(int minX, int maxX, int minY, int maxY, TerrainPositionsFromCameraBoundaries terrainPositionsFromCameraBoundaries);

        // ReSharper disable once InconsistentNaming
        protected ObjectPoolerManager _objectPoolerManager;
        
        public AObjectPoolerDisplayer(ObjectPoolerManager objectPoolerManager)
        {
            _objectPoolerManager = objectPoolerManager;
        }
    }
}