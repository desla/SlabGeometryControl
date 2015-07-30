using System;
using Alvasoft.SlabBuilder;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    /// <summary>
    /// Поперечная кривизна (боковое искривление).
    /// </summary>
    public class LateralCurvatureLeftAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "left_side_leteral_curvature";
        }

        public double CalculateValue(ISlabModel aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            // вычисляем середину слитка.
            var indent = 150.0; // 15 см.
            var coordinateY = (aSlabModel.GetBottomLimit() + aSlabModel.GetTopLimit())/2;
            var leftPoint = aSlabModel.GetLeftSidePoint(coordinateY, indent);
            var rightPoint = aSlabModel.GetLeftSidePoint(coordinateY, aSlabModel.GetLengthLimit() - indent);
            var middlePoint = aSlabModel.GetLeftSidePoint(coordinateY, aSlabModel.GetLengthLimit() / 2);

            return Math.Round(middlePoint.DistanceToLine(leftPoint, rightPoint), 4);
        }
    }
}
