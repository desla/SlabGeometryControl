using System;
using System.Linq;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms {
    public class MiddleDiameterAlgorithm : IDimentionAlgorithm {
        public double CalculateValue(SlabModelImpl aSlabModel) {
            if (aSlabModel == null) {
                throw new ArgumentNullException("MiddleDiameterAlgorithm, CalculateValue: Модель слитка равна null.");
            }

            if (aSlabModel.Diameters == null || aSlabModel.Diameters.Length == 0) {
                throw new ArgumentNullException("MiddleDiameterAlgorithm, CalculateValue: Диаметры отсутствуют.");
            }

            var firstPoint = aSlabModel.CenterLine.First();
            var lastPoint = aSlabModel.CenterLine.Last();
            var middleDistance = firstPoint.Z + (lastPoint.Z - firstPoint.Z) / 2.0;
            for (var i = 0; i < aSlabModel.CenterLine.Length; ++i) {
                if (aSlabModel.CenterLine[i].Z >= middleDistance) {
                    return Math.Round(aSlabModel.Diameters[i] - 0.2, 4);
                }
            }

            throw new ArgumentException("MiddleDiameterAlgorithm, CalculateValue: Не удалось найти среднюю точку.");
        }

        public string GetName() {
            return "middle_diameter";
        }
    }
}
