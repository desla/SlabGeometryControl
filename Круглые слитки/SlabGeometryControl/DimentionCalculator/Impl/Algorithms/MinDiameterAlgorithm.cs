using System;
using System.Linq;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms {
    public class MinDiameterAlgorithm : IDimentionAlgorithm {
        public double CalculateValue(SlabModelImpl aSlabModel) {
            if (aSlabModel == null) {
                throw new ArgumentNullException("MinDiameterAlgorithm, CalculateValue: Модель слитка равна null.");
            }

            if (aSlabModel.Diameters == null || aSlabModel.Diameters.Length == 0) {
                throw new ArgumentNullException("MinDiameterAlgorithm, CalculateValue: Диаметры отсутствуют.");
            }
            
            return Math.Round(aSlabModel.Diameters.Min(), 4);
        }

        public string GetName() {
            return "min_diameter";
        }
    }
}
