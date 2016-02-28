using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

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

        public double CalculateValue(SlabModelImpl aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            return 0;
        }
    }
}
