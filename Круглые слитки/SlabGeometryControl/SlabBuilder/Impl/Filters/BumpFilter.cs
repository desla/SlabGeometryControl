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
