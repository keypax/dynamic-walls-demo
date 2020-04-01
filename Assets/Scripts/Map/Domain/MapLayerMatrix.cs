namespace Map.Domain
{
    /**
     * Two-dimensional matrix (imagine it as such a large chessboard) where we store various types of data (e.g. where are buildings, trees, forest, etc.).
     * We can easily create any matrix layer and later check in the code "if there is a forest in this place, but there is no tree, move the unit to this position.
     */
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