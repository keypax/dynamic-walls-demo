namespace Map.Domain
{
    public class MapLayerMatrix
    {
        public short Width { get; }
        public short Height { get; }
        public long LastUpdate { get; set; }

        public byte[,] Matrix { get; }

        public MapLayerMatrix(short width, short height)
        {
            Width = width;
            Height = height;
            
            Matrix = new byte[Width, Height];
        }
    }
}