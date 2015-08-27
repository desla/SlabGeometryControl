using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    /// <summary>
    /// Вогнутость посередине слитка.
    /// </summary>
    public class ConcavityMiddleTopAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "concavityMiddleTopSide";
        }

        public double CalculateValue(SlabModelImpl aSlabModel)
        {
            var topLine = aSlabModel.TopLines[aSlabModel.TopLines.Length/2];
            var xPoints = new double[topLine.Length];
            var yPoints = new double[topLine.Length];
            for (var i = 0; i < topLine.Length; ++i) {
                xPoints[i] = topLine[i].Z;
                yPoints[i] = topLine[i].Y;
            }


            var middlePoint = topLine.Length/2;
            var saddlePoints = ConvexHull.Build(xPoints, yPoints, 1);
            for (var i = 0; i < saddlePoints.Length - 1; ++i) {
                if (middlePoint >= saddlePoints[i] && middlePoint <= saddlePoints[i + 1]) {
                    var leftPoint = topLine[saddlePoints[i]];
                    var rightPoint = topLine[saddlePoints[i + 1]];
                    var distance = topLine[middlePoint].DistanceToLine(leftPoint, rightPoint);
                    return Math.Round(distance, 4, MidpointRounding.ToEven);
                }
            }

            return 0;
        }
    }
}
