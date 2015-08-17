using System;
using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface.Filters
{
    public class PickFilter
    {
        public static void Filter(SlabModel3D aSlab)
        {
            if (aSlab == null) {
                return;
            }

            if (aSlab.TopLines != null) {
                for (var i = 0; i < aSlab.TopLines.Length; ++i) {
                    FilterLeftView(aSlab.TopLines[i]);
                }
            }

            if (aSlab.BottomLines != null) {
                for (var i = 0; i < aSlab.BottomLines.Length; ++i) {
                    FilterLeftView(aSlab.BottomLines[i]);
                }
            }

            if (aSlab.LeftLines != null) {
                for (var i = 0; i < aSlab.LeftLines.Length; ++i) {
                    FilterTopView(aSlab.LeftLines[i]);
                }
            }

            if (aSlab.RightLines != null) {
                for (var i = 0; i < aSlab.RightLines.Length; ++i) {
                    FilterTopView(aSlab.RightLines[i]);
                }
            }
        }

        private static void FilterTopView(SlabPoint[] aLine)
        {
            if (aLine == null) {
                return;
            }

            for (var i = 0; i < aLine.Length - 2; ++i) {
                var a = aLine[i];
                var b = aLine[i + 1];
                var c = aLine[i + 2];
                if (IsPick(a.X, b.X, c.X)) {
                    b.X = (a.X + c.X) / 2;                    
                }
            }
        }

        private static void FilterLeftView(SlabPoint[] aLine)
        {
            if (aLine == null) {
                return;
            }

            for (var i = 0; i < aLine.Length - 2; ++i) {
                var a = aLine[i];
                var b = aLine[i + 1];
                var c = aLine[i + 2];
                if (IsPick(a.Y, b.Y, c.Y)) {
                    b.Y = (a.Y + c.Y) / 2;                    
                }
            }
        }

        private static bool IsPick(double a, double b, double c)
        {
            if (Math.Min(a, c) - 0.3 <= b && b <= Math.Max(a, c) + 0.3) {
                return false;
            }

            return true;
        }
    }
}
