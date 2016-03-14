using System;
using System.Linq;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms {
    public class FrontDiameterAlgorithm : IDimentionAlgorithm {
        private double frontIndent = 150; // отступ от торца слитка в мм.

        public double CalculateValue(SlabModelImpl aSlabModel) {
            if (aSlabModel == null) {
                throw new ArgumentNullException("FrontDiameterAlgorithm, CalculateValue: Модель слитка равна null.");
            }

            if (aSlabModel.Diameters == null || aSlabModel.Diameters.Length == 0) {
                throw new ArgumentNullException("FrontDiameterAlgorithm, CalculateValue: Диаметры отсутствуют.");
            }

            for (var i = 0; i < aSlabModel.CenterLine.Length; ++i) {
                if (aSlabModel.CenterLine[i].Z - aSlabModel.CenterLine[0].Z >= frontIndent) {
                    return Math.Round(aSlabModel.Diameters[i], 4);
                }
            }

            throw new ArgumentException("FrontDiameterAlgorithm, CalculateValue: Нельзя вычислить диаметр с отступом в " + frontIndent + " мм.");
        }

        public string GetName() {
            return "front_diameter";
        }
    }
}
