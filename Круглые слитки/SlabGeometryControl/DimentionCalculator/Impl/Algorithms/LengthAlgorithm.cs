using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class LengthAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "length";
        }

        public double CalculateValue(SlabModelImpl aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            return Math.Round(aSlabModel.GetLengthLimit(), 4);
        }
    }
}
