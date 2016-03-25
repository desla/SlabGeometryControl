using System;
using System.Collections.Generic;
using Alvasoft.Utils.Mathematic3D;

namespace Alvasoft.SlabBuilder.Impl.Filters
{
    /// <summary>
    /// Фильтр всплесков показаний, которые получаются в начале и в конце измерения.
    /// </summary>
    public class SplashFilter
    {
        // Число точек от начана и от конца, которые будут фильтроваться.
        private const int MAX_POINT_COUNT = 30;

        // Значение производной, после которого считает, что это всплеск.
        private const double splashIndex = 4.5;

        public static void Filter(SlabModelImpl aSlab)
        {
            if (aSlab == null) {
                return;
            }            

            if (aSlab.TopSensorLine != null) {                
                FilterLeftView(ref aSlab.TopSensorLine);                
            }

            if (aSlab.BottomSensorLine != null) {                
                FilterLeftView(ref aSlab.BottomSensorLine);                
            }

            if (aSlab.LeftSensorLine != null) {                
                FilterTopView(ref aSlab.LeftSensorLine);
            }

            if (aSlab.RightSensorLine != null) {                
                FilterTopView(ref aSlab.RightSensorLine);
            }
        }

        private static void FilterTopView(ref Point3D[] aLine)
        {
            if (aLine == null) {
                return;
            }

            var startDeletetIndex = int.MinValue;
            for (var i = MAX_POINT_COUNT; i > 0 && i < aLine.Length; --i) {
                if (IsSplash(aLine[i - 1].Z, aLine[i - 1].X, aLine[i].Z, aLine[i].X)) {
                    startDeletetIndex = i;
                    break;
                }
            }

            var endDeletetIndex = int.MaxValue;
            for (var i = aLine.Length - MAX_POINT_COUNT; i > 0 && i < aLine.Length; ++i) {
                if (IsSplash(aLine[i - 1].Z, aLine[i - 1].X, aLine[i].Z, aLine[i].X)) {
                    endDeletetIndex = i - 1;
                    break;
                }
            }

            if (startDeletetIndex != int.MinValue || endDeletetIndex != int.MaxValue) {
                var points = new List<Point3D>();
                for (var i = 0; i < aLine.Length; ++i) {
                    if (i >= startDeletetIndex && i <= endDeletetIndex) {
                        points.Add(aLine[i]);
                    }
                }

                aLine = points.ToArray();
            }           
        }

        private static void FilterLeftView(ref Point3D[] aLine)
        {
            if (aLine == null) {
                return;
            }

            var startDeletetIndex = int.MinValue;
            for (var i = MAX_POINT_COUNT; i > 0 && i < aLine.Length; --i) {
                if (IsSplash(aLine[i - 1].Z, aLine[i - 1].Y, aLine[i].Z, aLine[i].Y)) {
                    startDeletetIndex = i;
                    break;
                }
            }

            var endDeletetIndex = int.MaxValue;
            for (var i = aLine.Length - MAX_POINT_COUNT; i > 0 && i < aLine.Length; ++i) {
                if (IsSplash(aLine[i - 1].Z, aLine[i - 1].Y, aLine[i].Z, aLine[i].Y)) {
                    endDeletetIndex = i - 1;
                    break;
                }
            }

            if (startDeletetIndex != int.MinValue || endDeletetIndex != int.MaxValue) {
                var points = new List<Point3D>();
                for (var i = 0; i < aLine.Length; ++i) {
                    if (i >= startDeletetIndex && i <= endDeletetIndex) {
                        points.Add(aLine[i]);
                    }
                }

                aLine = points.ToArray();
            }
        }

        private static bool IsSplash(double aAx, double aAy, double aBx, double aBy)
        {
            var splash = (aAy - aBy)/(aAx - aBx);
            return Math.Abs(splash) >= splashIndex;
        }
    }
}
