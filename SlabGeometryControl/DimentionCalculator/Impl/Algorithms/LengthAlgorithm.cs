using System;
using Alvasoft.SlabBuilder;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class LengthAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "length";
        }

        public double CalculateValue(ISlabModel aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            return Math.Round(aSlabModel.GetLengthLimit(), 4);
        }
    }
}
