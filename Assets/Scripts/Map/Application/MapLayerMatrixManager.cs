using System;
using Map.Domain;

namespace Map.Application
{
    public class MapLayerMatrixManager
    {
        public void Add(MapLayerMatrix mapLayerMatrix, short pointX, short pointY, byte radius, sbyte forcedValue = -1)
        {
            for (int x = pointX-radius; x < pointX+radius+1; x++)
            {
                if (x < 0 || x > mapLayerMatrix.Width - 1)
                {
                    continue;
                }
                
                for (int y = pointY-radius; y < pointY+radius+1; y++)
                {
                    if (y < 0 || y > mapLayerMatrix.Height - 1)
                    {
                        continue;
                    }

                    if (forcedValue == -1)
                    {
                        mapLayerMatrix.Matrix[x, y] += (byte) ((radius+1) - Math.Max(Math.Abs(pointX - x), Math.Abs(pointY - y)));
                    }
                    else
                    {
                        mapLayerMatrix.Matrix[x, y]  = (byte) forcedValue;
                    }
                }
            }

            mapLayerMatrix.LastUpdate = DateTime.Now.Ticks;
        }
        
        public void Remove(MapLayerMatrix mapLayerMatrix, short pointX, short pointY, byte radius, sbyte forcedValue = -1)
        {
            for (int x = pointX-radius; x < pointX+radius+1; x++)
            {
                if (x < 0 || x > mapLayerMatrix.Width - 1)
                {
                    continue;
                }
                
                for (int y = pointY-radius; y < pointY+radius+1; y++)
                {
                    if (y < 0 || y > mapLayerMatrix.Height - 1)
                    {
                        continue;
                    }

                    if (forcedValue == -1)
                    {
                        mapLayerMatrix.Matrix[x, y] -= (byte) ((radius + 1) - Math.Max(Math.Abs(pointX - x), Math.Abs(pointY - y)));
                    }
                    else
                    {
                        mapLayerMatrix.Matrix[x, y]  = (byte) forcedValue;
                    }
                }
            }
            
            mapLayerMatrix.LastUpdate = DateTime.Now.Ticks;
        }
        
        public byte Get(MapLayerMatrix mapLayerMatrix, short pointX, short pointY)
        {
            if (pointX < 0 || pointY < 0 || pointX > mapLayerMatrix.Width-1 || pointY > mapLayerMatrix.Height-1)
            {
                return 0;
            }
            
            return mapLayerMatrix.Matrix[pointX, pointY];
        }
    }
}