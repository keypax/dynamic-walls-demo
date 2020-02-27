using CustomCamera.Domain;

namespace ObjectPooler.Domain
{
    public interface IObjectPoolerDisplayer
    {
        bool IsDynamic();
        void Display(int minX, int maxX, int minY, int maxY, TerrainPositionsFromCameraBoundaries terrainPositionsFromCameraBoundaries);
    }
}