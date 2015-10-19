using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class HeightAlgorithm : IDimentionAlgorithm
    {
        private static Random rnd = new Random(7654321);

        public string GetName()
        {
            return "height";
        }

        public double CalculateValue(SlabModelImpl aSlabModel)
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

            var height = Math.Round(topPoint.Y - bottomPoint.Y, 4, MidpointRounding.ToEven);
            //if (Math.Abs(height - 205) <= 10) {
            //    height = 205 + (double)rnd.Next(49) / 100.0 * (rnd.Next() % 2 == 0 ? -1 : 1);
            //}

            return height;
        }
    }
}
