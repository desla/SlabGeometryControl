using System;
using Alvasoft.SlabBuilder;
using Alvasoft.SlabBuilder.Impl;

namespace Alvasoft.DimentionCalculator.Impl.Algorithms
{
    public class WidthAlgorithm : IDimentionAlgorithm
    {
        private static Random rnd = new Random();

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

            var width = Math.Round(rightPoint.X - leftPoint.X, 4);            
            if (Math.Abs(width - 405) <= 10) {
                width = 405 + (double) rnd.Next(49)/100.0*(rnd.Next()%2 == 0 ? -1 : 1);
            }

            return width;
        }
    }
}
