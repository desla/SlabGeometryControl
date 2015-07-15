using System;
using Alvasoft.SlabBuilder;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class WidthAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "width";
        }

        public double CalculateValue(ISlabModel aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            // вычисляем середину слитка.
            var positionY = 0.5 * (aSlabModel.GetTopLimit() + aSlabModel.GetBottomLimit());
            // отступаем 10 см от торца слитка.
            var positionZ = 100;

            var leftPoint = aSlabModel.GetLeftSidePoint(positionY, positionZ);
            var rightPoint = aSlabModel.GetRightSidePoint(positionY, positionZ);

            return rightPoint.X - leftPoint.X;
        }
    }
}
