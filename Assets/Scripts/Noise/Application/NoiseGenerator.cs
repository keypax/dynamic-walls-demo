using UnityEngine;

namespace Noise.Application
{
    public class NoiseGenerator
    {
        private int _width;
        private int _height;
        
        public NoiseGenerator(int width, int height)
        {
            _width = width;
            _height = height;
        }
        
        public float Generate(int pointX, int pointY, float scale, int offset = 0)
        {
            return Mathf.PerlinNoise(
                (float) pointX / _width * scale + offset,
                (float) pointY / _height * scale + offset
            );
        }
    }
}