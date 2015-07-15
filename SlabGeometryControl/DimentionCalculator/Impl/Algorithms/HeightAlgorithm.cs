using System;
using Alvasoft.SlabBuilder;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class HeightAlgorithm : IDimentionAlgorithm
    {
        public string GetName()
        {
            return "height";
        }

        public double CalculateValue(ISlabModel aSlabModel)
        {
            if (aSlabModel == null) {
                throw new ArgumentNullException("aSlabModel");
            }

            // вычисляем середину слитка.
            var positionX = 0.5 * (aSlabModel.GetRightLimit() + aSlabModel.GetLeftLimit());
            // отступаем 10 см от торца слитка.
            var positionZ = 100;

            var topPoint = aSlabModel.GetTopSidePoint(positionX, positionZ);
            var bottomPoint = aSlabModel.GetBottomSidePoint(positionX, positionZ);

            return topPoint.Y - bottomPoint.Y;
        }
    }
}
