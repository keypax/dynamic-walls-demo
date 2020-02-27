using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CustomCamera.Domain
{
    public class TerrainPositionsFromCameraBoundaries
    {
        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        private Vector2[] _polyPoints = new Vector2[4];

        //it looks stupid but IT WORKS! ;)
        public TerrainPositionsFromCameraBoundaries(Vector3 topLeft, Vector3 topRight, Vector3 bottomLeft, Vector3 bottomRight)
        {
            _minX = Mathf.Infinity;
            _maxX = 0;
            _minY = Mathf.Infinity;
            _maxY = 0;

            //minX
            if (_minX > topLeft.x)
            {
                _minX = topLeft.x;
            }

            if (_minX > topRight.x)
            {
                _minX = topRight.x;
            }

            if (_minX > bottomLeft.x)
            {
                _minX = bottomLeft.x;
            }

            if (_minX > bottomRight.x)
            {
                _minX = bottomRight.x;
            }

            //maxX
            if (_maxX < topLeft.x)
            {
                _maxX = topLeft.x;
            }

            if (_maxX < topRight.x)
            {
                _maxX = topRight.x;
            }

            if (_maxX < bottomLeft.x)
            {
                _maxX = bottomLeft.x;
            }

            if (_maxX < bottomRight.x)
            {
                _maxX = bottomRight.x;
            }

            //minY
            if (_minY > topLeft.z)
            {
                _minY = topLeft.z;
            }

            if (_minY > topRight.z)
            {
                _minY = topRight.z;
            }

            if (_minY > bottomLeft.z)
            {
                _minY = bottomLeft.z;
            }

            if (_minY > bottomRight.z)
            {
                _minY = bottomRight.z;
            }

            //maxY
            if (_maxY < topLeft.z)
            {
                _maxY = topLeft.z;
            }

            if (_maxY < topRight.z)
            {
                _maxY = topRight.z;
            }

            if (_maxY < bottomLeft.z)
            {
                _maxY = bottomLeft.z;
            }

            if (_maxY < bottomRight.z)
            {
                _maxY = bottomRight.z;
            }

            Debug.DrawLine(topLeft, topRight, Color.blue);
            Debug.DrawLine(topRight, bottomRight, Color.green);
            Debug.DrawLine(bottomRight, bottomLeft, Color.red);
            Debug.DrawLine(bottomLeft, topLeft, Color.yellow);

            var points = new[]
            {
                new Vector2(topLeft.x, topLeft.z),
                new Vector2(topRight.x, topRight.z),
                new Vector2(bottomLeft.x, bottomLeft.z),
                new Vector2(bottomRight.x, bottomRight.z),
            };

            //points = ResizePoints(points);
            CalculateContour(points.ToList());
        }

        public float GetMinX()
        {
            return _minX;
        }

        public float GetMinY()
        {
            return _minY;
        }

        public float GetMaxX()
        {
            return _maxX;
        }

        public float GetMaxY()
        {
            return _maxY;
        }

        public bool IsInsidePolygon(Vector2 point)
        {
            var j = _polyPoints.Length - 1;
            var inside = false;
            for (int i = 0; i < _polyPoints.Length; j = i++)
            {
                var pi = _polyPoints[i];
                var pj = _polyPoints[j];
                if (((pi.y <= point.y && point.y < pj.y) || (pj.y <= point.y && point.y < pi.y)) &&
                    (point.x < (pj.x - pi.x) * (point.y - pi.y) / (pj.y - pi.y) + pi.x))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        //Modified code from: https://stackoverflow.com/questions/14392072/how-to-draw-a-polygon-from-a-set-of-unordered-points
        //@author: https://stackoverflow.com/users/210709/iabstract
        private void CalculateContour(List<Vector2> points)
        {

            // locate lower-leftmost point
            int hull = 0;
            int i;
            for (i = 1; i < points.Count; i++)
            {
                if (ComparePoint(points[i], points[hull]))
                {
                    hull = i;
                }
            }

            // wrap contour
            var outIndices = new int[points.Count];
            int endPt;
            i = 0;
            do
            {
                outIndices[i++] = hull;
                endPt = 0;
                for (int j = 1; j < points.Count; j++)
                {
                    if (hull == endPt || IsLeft(points[hull], points[endPt], points[j]))
                    {
                        endPt = j;
                    }
                }

                hull = endPt;
            } while (endPt != outIndices[0]);

            // build countour points
            int results = i;
            for (i = 0; i < results; i++)
            {
                _polyPoints[i] = points[outIndices[i]];
            }
        }

        private bool ComparePoint(Vector2 a, Vector2 b)
        {
            if (a.x < b.x) return true;
            if (a.x > b.x) return false;
            if (a.y < b.y) return true;
            if (a.y > b.y) return false;

            return false;
        }

        private bool IsLeft(Vector2 a, Vector2 b, Vector2 c)
        {
            var u1 = b.x - a.x;
            var v1 = b.y - a.y;
            var u2 = c.x - a.x;
            var v2 = c.y - a.y;

            return u1 * v2 - v1 * u2 < 0;
        }
    }
}