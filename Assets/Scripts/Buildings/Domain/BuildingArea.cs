namespace Buildings.Domain
{
    public class BuildingArea
    {
        public short MinX { get; }
        public short MaxX { get; }
        public short MinY { get; }
        public short MaxY { get; }

        public BuildingArea(short minX, short maxX, short minY, short maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }
    }
}