using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    /// <summary>
    /// Поперечная кривизна (боковое искривление).
    /// </summary>
    public class LateralCurvatureRightAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "right_side_leteral_curvature";
        }

        public double CalculateValue(SlabModelImpl aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            // вычисляем середину слитка.
            var indent = 150.0; // 15 см.
            var coordinateY = (aSlabModel.GetBottomLimit() + aSlabModel.GetTopLimit())/2;
            var leftPoint = aSlabModel.GetRightSidePoint(coordinateY, indent);
            var rightPoint = aSlabModel.GetRightSidePoint(coordinateY, aSlabModel.GetLengthLimit() - indent);
            var middlePoint = aSlabModel.GetRightSidePoint(coordinateY, aSlabModel.GetLengthLimit()/2);

            return Math.Round(middlePoint.DistanceToLine(leftPoint, rightPoint), 4);
        }
    }
}
