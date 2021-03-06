﻿using System;
using System.Linq;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms {
    public class FrontMiddleDiameterAlgorithm : IDimentionAlgorithm {
        private double frontIndent = 500; // отступ от торца слитка в мм.

        public double CalculateValue(SlabModelImpl aSlabModel) {
            if (aSlabModel == null) {
                throw new ArgumentNullException("FrontMiddleDiameterAlgorithm, CalculateValue: Модель слитка равна null.");
            }

            if (aSlabModel.Diameters == null || aSlabModel.Diameters.Length == 0) {
                throw new ArgumentNullException("FrontMiddleDiameterAlgorithm, CalculateValue: Диаметры отсутствуют.");
            }

            var firstPoint = aSlabModel.CenterLine.First();
            var lastPoint = aSlabModel.CenterLine.Last();
            var middleDistance = firstPoint.Z + (lastPoint.Z - firstPoint.Z) / 2.0;
            var controlDistance = middleDistance - (middleDistance - firstPoint.Z - frontIndent) / 2.0;

            for (var i = 0; i < aSlabModel.CenterLine.Length; ++i) {
                if (aSlabModel.CenterLine[i].Z >= controlDistance) {
                    return Math.Round(aSlabModel.Diameters[i] - 0.2, 4);
                }
            }

            throw new ArgumentException("FrontMiddleDiameterAlgorithm, CalculateValue: Нельзя вычислить диаметр с отступом в " + frontIndent + " мм.");
        }

        public string GetName() {
            return "front_middle_diameter";
        }
    }
}
