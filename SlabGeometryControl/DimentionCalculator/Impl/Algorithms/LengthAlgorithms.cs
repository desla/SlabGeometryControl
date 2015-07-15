using System;
using Alvasoft.SlabBuilder;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class LengthAlgorithms : IDimentionAlgorithm
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

            return aSlabModel.GetLengthLimit();
        }
    }
}
