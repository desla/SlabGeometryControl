using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class SlopeAngleAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "angle";
        }

        public double CalculateValue(SlabModelImpl aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            var indent = 30.0; // 3 см.
            // вычисляем середину слитка.
            var coordinateX = 0;
            var leftPoint = aSlabModel.GetTopSidePoint(coordinateX, indent);
            var rightPoint = aSlabModel.GetTopSidePoint(coordinateX, aSlabModel.GetLengthLimit() - indent);

            var hypotenuse = leftPoint.DistanceToPoint(rightPoint);

            var angle = Math.Asin((leftPoint.Y - rightPoint.Y) / hypotenuse);

            return 180.0 * angle / Math.PI;
        }
    }
}
