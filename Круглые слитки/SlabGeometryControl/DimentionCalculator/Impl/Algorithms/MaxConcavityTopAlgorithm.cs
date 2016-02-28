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
            return 0;
        }
    }
}
