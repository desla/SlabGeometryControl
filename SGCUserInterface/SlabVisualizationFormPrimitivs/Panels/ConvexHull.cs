using System;
using System.Collections.Generic;
using System.Linq;

namespace SGCUserInterface.SlabVisualizationFormPrimitivs.Panels
{
    public static class ConvexHull
    {
        /// <summary>
        /// Строит выпуклую оболочку по точкам.
        /// </summary>
        /// <param name="aXPoints">Координаты X.</param>
        /// <param name="aYPoints">Координаты Y.</param>
        /// <param name="aSideValue">Направление выпуклой оболочки (-1 или 1).</param>
        /// <returns>индексы седловых точек.</returns>
        public static int[] Build(double[] aXPoints, double[] aYPoints, int aSideValue)
        {
            CheckRegulations(aXPoints, aYPoints, aSideValue);

            var saddlePoints = new Stack<int>();
            var preLastPoint = 0;
            var lastPoint = 1;
            saddlePoints.Push(preLastPoint);
            saddlePoints.Push(lastPoint);

            var pointsCount = aXPoints.Length;
            for (var i = 2; i < pointsCount; ++i) {
                while (lastPoint != 0 && 
                       !IsSameDirection(aXPoints[preLastPoint], 
                                        aYPoints[preLastPoint],
                                        aXPoints[i],
                                        aYPoints[i],
                                        aXPoints[lastPoint],
                                        aYPoints[lastPoint],
                                        aSideValue)) {
                    lastPoint = preLastPoint;
                    saddlePoints.Pop();
                    if (preLastPoint == 0) {
                        preLastPoint = -1;
                    }
                    else {
                        preLastPoint = saddlePoints.ElementAt(1);
                    }
                }

                preLastPoint = lastPoint;
                lastPoint = i;
                saddlePoints.Push(i);
            }

            return saddlePoints.ToArray();
        }

        private static bool IsSameDirection(
            double aPx, 
            double aPy, 
            double aQx, 
            double aQy, 
            double aDx, 
            double aDy, 
            int aSideValue)
        {
            var a = aPy - aQy;
            var b = aQx - aPx;
            var c = -a*aPx - b*aPy;

            return ((a*aDx + b*aDy + c)*aSideValue) > 0;
        }

        private static void CheckRegulations(double[] aXPoints, double[] aYPoints, int aSideValue)
        {
            if (aXPoints == null) {
                throw new ArgumentNullException("aXPoints");
            }

            if (aYPoints == null) {
                throw new ArgumentNullException("aYPoints");
            }

            if (aXPoints.Length != aYPoints.Length) {
                throw new ArgumentException("Длина массивов точек не совпадает.");
            }

            if (aSideValue == 0) {
                throw new ArgumentException("Коэфициент направления выпуклой оболочки не должен быть равен 0.");
            }
        }
    }
}
