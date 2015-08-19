using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    /// <summary>
    /// Максимальная вогнутость.
    /// </summary>
    public class MaxConcavityTopAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "maxConcavityTopSide";
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


            var saddlePoints = ConvexHull.Build(xPoints, yPoints, 1);
            var leftSaddlePoint = 0;
            var maxConcavity = double.MinValue;
            for (var i = 1; i < topLine.Length - 1; ++i) {
                if (i == saddlePoints[leftSaddlePoint + 1]) {
                    maxConcavity = Math.Max(maxConcavity, 0);
                    leftSaddlePoint++;
                }
                else {
                    var distance = topLine[i].DistanceToLine(
                        topLine[saddlePoints[leftSaddlePoint]],
                        topLine[saddlePoints[leftSaddlePoint + 1]]);
                    maxConcavity = Math.Max(maxConcavity, distance);
                }
            }

            return Math.Round(maxConcavity, 4, MidpointRounding.ToEven);
        }
    }
}
