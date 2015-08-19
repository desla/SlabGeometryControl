using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class RotateAngleAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "rotateAngle";
        }

        public double CalculateValue(SlabModelImpl aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            var indent = 30.0; // 3 см.
            // вычисляем середину слитка.
            var coordinateY = 0;
            var leftPoint = aSlabModel.GetLeftSidePoint(coordinateY, indent);
            var rightPoint = aSlabModel.GetLeftSidePoint(coordinateY, aSlabModel.GetLengthLimit() - indent);

            var hypotenuse = leftPoint.DistanceToPoint(rightPoint);

            var angle = Math.Asin((leftPoint.X - rightPoint.X) / hypotenuse);

            return 180.0 * angle / Math.PI;
        }
    }
}
