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
        private const double splashIndex = 0.6;

        public static void Filter(SlabModelImpl aSlab)
        {
            if (aSlab == null) {
                return;
            }

            if (aSlab.TopLines != null) {
                for (var i = 0; i < aSlab.TopLines.Length; ++i) {
                    FilterLeftView(ref aSlab.TopLines[i]);
                }
            }

            if (aSlab.BottomLines != null) {
                for (var i = 0; i < aSlab.BottomLines.Length; ++i) {
                    FilterLeftView(ref aSlab.BottomLines[i]);
                }
            }

            if (aSlab.LeftLines != null) {
                for (var i = 0; i < aSlab.LeftLines.Length; ++i) {
                    FilterTopView(ref aSlab.LeftLines[i]);
                }
            }

            if (aSlab.RightLines != null) {
                for (var i = 0; i < aSlab.RightLines.Length; ++i) {
                    FilterTopView(ref aSlab.RightLines[i]);
                }
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
