using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class WidthAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "width";
        }

        public double CalculateValue(SlabModelImpl aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            // вычисляем середину слитка.
            //var positionY = 0.5 * (aSlabModel.GetTopLimit() + aSlabModel.GetBottomLimit());
            var positionY = 0;
            // отступаем 2 см от торца слитка.
            var positionZ = 20;

            var leftPoint = aSlabModel.GetLeftSidePoint(positionY, positionZ);
            var rightPoint = aSlabModel.GetRightSidePoint(positionY, positionZ);

            return Math.Round(rightPoint.X - leftPoint.X, 4);
        }
    }
}
