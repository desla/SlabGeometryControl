using System;
using System.Collections.Generic;
using System.Drawing;
using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface.Filters
{
    public class ShiftingFilter : IFilter
    {
        public PointF[] Filter(PointF[] aPoints)
        {
            var points = new List<PointF>(aPoints).ToArray();
            var middleDifference = 0.0;
            for (var i = 0; i < points.Length - 1; ++i) {
                middleDifference += Math.Abs(points[i].Y - points[i + 1].Y);
            }
            middleDifference /= (points.Length - 1);
            for (var i = 0; i < points.Length - 1; ++i) {
                var difference = points[i].Y - points[i + 1].Y;
                if (Math.Abs(difference) > middleDifference) {
                    for (var j = i; j < points.Length; ++j) {
                        points[j].Y += difference / 2;
                    }
                }
            }

            return points;
        }
    }
}
