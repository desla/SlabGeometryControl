using System;
using Alvasoft.Utils.Mathematic3D;

namespace Alvasoft.SlabBuilder.Impl.Filters
{
    public class BumpFilter
    {
        private const double MAX_RADIUS = 1.5;
        
        public static void Filter(SlabModelImpl aSlab)
        {
            if (aSlab == null) {
                return;
            }

            if (aSlab.CenterLine != null) {
                FilterBumps(aSlab.CenterLine);
            }            
        }

        private static void FilterBumps(Point3D[] aCenterLine) {
            for (var i = 0; i < aCenterLine.Length - 1; ++i) {
                if (IsBump(aCenterLine[i], aCenterLine[i + 1])) {
                    var dx = aCenterLine[i].X - aCenterLine[i + 1].X;
                    var dy = aCenterLine[i].Y - aCenterLine[i + 1].Y;
                    SmoothBump(aCenterLine, i + 1, dx, dy);
                }
            }
        }

        private static void SmoothBump(Point3D[] aCenterLine, int aStartPosition, double aDx, double aDy) {
            for (var i = aStartPosition; i < aCenterLine.Length; ++i) {
                aCenterLine[i].X += aDx;
                aCenterLine[i].Y += aDy;
            }
        }

        private static bool IsBump(Point3D aA, Point3D aB) {            
            var distance = Math.Sqrt((aA.X-aB.X)* (aA.X - aB.X) + (aA.Y - aB.Y) * (aA.Y - aB.Y));

            if (distance > MAX_RADIUS) {
                return true;
            }

            return false;
        }
    }
}
