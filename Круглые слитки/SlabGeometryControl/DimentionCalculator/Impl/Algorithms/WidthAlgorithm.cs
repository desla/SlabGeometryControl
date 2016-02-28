using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class WidthAlgorithm : IDimentionAlgorithm
    {
        private static Random rnd = new Random(1234567);

        public string GetName()
        {
            return "width";
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
