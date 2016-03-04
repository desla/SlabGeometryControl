using System;
using System.Linq;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms {
    public class MaxDiameterAlgorithm : IDimentionAlgorithm {
        public double CalculateValue(SlabModelImpl aSlabModel) {
            if (aSlabModel == null) {
                throw new ArgumentNullException("MaxDiameterAlgorithm, CalculateValue: Модель слитка равна null.");
            }

            if (aSlabModel.Diameters == null || aSlabModel.Diameters.Length == 0) {
                throw new ArgumentNullException("MaxDiameterAlgorithm, CalculateValue: Диаметры отсутствуют.");
            }
            
            return Math.Round(aSlabModel.Diameters.Max(), 4);
        }

        public string GetName() {
            return "max_diameter";
        }
    }
}
