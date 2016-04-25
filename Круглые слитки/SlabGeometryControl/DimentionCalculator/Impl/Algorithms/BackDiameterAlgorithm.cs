using System;
using System.Linq;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms {
    public class BackDiameterAlgorithm : IDimentionAlgorithm {
        private double backIndent = 500; // отступ от торца слитка.

        public double CalculateValue(SlabModelImpl aSlabModel) {
            if (aSlabModel == null) {
                throw new ArgumentNullException("BackDiameterAlgorithm, CalculateValue: Модель слитка равна null.");
            }

            if (aSlabModel.Diameters == null || aSlabModel.Diameters.Length == 0) {
                throw new ArgumentNullException("BackDiameterAlgorithm, CalculateValue: Диаметры отсутствуют.");
            }

            var lastPoint = aSlabModel.CenterLine.Last();
            for (var i = aSlabModel.CenterLine.Length - 1; i >= 0; --i) {
                if (lastPoint.Z - aSlabModel.CenterLine[i].Z >= backIndent) {
                    return Math.Round(aSlabModel.Diameters[i] - 0.2, 4);
                }
            }

            throw new ArgumentException("BackDiameterAlgorithm, CalculateValue: Нельзя вычислить диаметр с отступом в " + backIndent + " мм.");
        }

        public string GetName() {
            return "back_diameter";
        }
    }
}
