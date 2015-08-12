using System.Collections.Generic;
using System.Drawing;

namespace SGCUserInterface.Filters
{
    public class RattleFilter : IFilter
    {
        public PointF[] Filter(PointF[] aPoints)
        {
            var points = new List<PointF>(aPoints).ToArray();
            for (var i = 0; i < points.Length; ++i) {
                var middleValue = CalcMiddleValue(i, aPoints);
                points[i].Y = (float) middleValue;
            }

            return points;
        }

        private double CalcMiddleValue(int aPosition, PointF[] aPoints)
        {
            var windowSize = 10;
            var middleValue = 0.0;
            var count = 0;
            for (var i = aPosition - windowSize; i < aPosition + windowSize; ++i) {
                if (i >= 0 && i < aPoints.Length) {
                    middleValue += aPoints[i].Y;
                    count++;
                }
            }

            return middleValue/count;
        }
    }
}
