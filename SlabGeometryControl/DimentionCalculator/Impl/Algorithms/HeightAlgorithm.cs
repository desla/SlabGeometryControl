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
            //var positionX = 0.5 * (aSlabModel.GetRightLimit() + aSlabModel.GetLeftLimit());
            var positionX = 0;
            // отступаем 2 см от торца слитка.
            var positionZ = 20;

            var topPoint = aSlabModel.GetTopSidePoint(positionX, positionZ);
            var bottomPoint = aSlabModel.GetBottomSidePoint(positionX, positionZ);
            Console.WriteLine(topPoint + " " + bottomPoint);
            return Math.Round(topPoint.Y - bottomPoint.Y, 4, MidpointRounding.ToEven);
        }
    }
}
