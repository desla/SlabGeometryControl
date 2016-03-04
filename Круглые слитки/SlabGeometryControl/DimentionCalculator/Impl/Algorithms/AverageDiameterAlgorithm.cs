using System;
using System.Linq;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms {
    public class AverageDiameterAlgorithm : IDimentionAlgorithm {
        public double CalculateValue(SlabModelImpl aSlabModel) {
            if (aSlabModel == null) {
                throw new ArgumentNullException("AverageDiameterAlgorithm, CalculateValue: Модель слитка равна null.");
            }

            if (aSlabModel.Diameters == null || aSlabModel.Diameters.Length == 0) {
                throw new ArgumentNullException("AverageDiameterAlgorithm, CalculateValue: Диаметры отсутствуют.");
            }
            
            return Math.Round(aSlabModel.Diameters.Average(), 4);
        }

        public string GetName() {
            return "average_diameter";
        }
    }
}
