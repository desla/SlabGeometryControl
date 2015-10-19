using System;
using Alvasoft.Utils.Mathematic3D;

namespace Alvasoft.SlabBuilder.Impl.Filters
{
    public class BumpFilter
    {
        private const int BUMP_POINTS_LENGTH = 1;
        private const double MAX_DIFFERENCE = 5;
        private const double MIN_BUMP = 0.2;

        public static void Filter(SlabModelImpl aSlab)
        {
            if (aSlab == null) {
                return;
            }

            var bottomLine = aSlab.BottomLines[aSlab.BottomLines.Length/2];
            var topLine = aSlab.TopLines[aSlab.TopLines.Length/2];
            for (var i = 0; i < bottomLine.Length - 1; ++i) {
                var bottomBump = GetBump(bottomLine[i], bottomLine[i + 1]);
                if (Math.Abs(bottomBump) >= MIN_BUMP) {
                    for (var j = -4; j <= 4; ++j) {
                        if (i + j >= 0 && i + j < bottomLine.Length - 1 &&
                            i+j < topLine.Length - 1) {
                            var topBump = GetBump(topLine[i+j], topLine[i+j+1]);
                            if (Math.Abs(topBump) >= MIN_BUMP) {
                                if (topBump * bottomBump > 0 && // если имеют одинаковые направления.
                                    Math.Abs(topBump - bottomBump) <= MAX_DIFFERENCE) { 
                                    FilterBump(bottomLine, i + 1, bottomBump);
                                    FilterBump(topLine, i + j + 1, topBump);
                                    break;
                                }
                            }
                        }                        
                    }
                }
            }
        }        

        private static double GetBump(Point3D aA, Point3D aB)
        {
            return aA.Y - aB.Y;
        }

        private static void FilterBump(Point3D[] aLine, int aStart, double aBump)
        {
            for (var i = aStart; i < aLine.Length; ++i) {
                aLine[i].Y += aBump;
            }
        }
    }
}
