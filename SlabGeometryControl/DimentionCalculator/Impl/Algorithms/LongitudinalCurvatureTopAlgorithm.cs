using System;
using Alvasoft.SlabBuilder;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    /// <summary>
    /// Поперечная кривизна (боковое искривление).
    /// </summary>
    public class LongitudinalCurvatureTopAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "top_side_longitudinal_curvature";
        }

        public double CalculateValue(ISlabModel aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }
            
            var indent = 150.0; // 15 см.
            // вычисляем середину слитка.
            var coordinateX = aSlabModel.GetLeftLimit() + 300; // отступ 30 см слева.
            var leftPoint = aSlabModel.GetTopSidePoint(coordinateX, indent);
            var rightPoint = aSlabModel.GetTopSidePoint(coordinateX, aSlabModel.GetLengthLimit() - indent);
            var middlePoint = aSlabModel.GetTopSidePoint(coordinateX, aSlabModel.GetLengthLimit()/2);

            return Math.Round(middlePoint.DistanceToLine(leftPoint, rightPoint), 4);
        }
    }
}
