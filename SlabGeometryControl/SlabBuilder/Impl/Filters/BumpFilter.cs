namespace Alvasoft.SlabBuilder.Impl.Filters
{
    public class BumpFilter
    {
        private const int BUMP_POINTS_LENGTH = 1;

        public static void Filter(SlabModelImpl aSlab)
        {
            if (aSlab == null) {
                return;
            }
            
            var line = aSlab.TopLines[aSlab.TopLines.Length/2];
            for (var i = 0; i < line.Length - 1; ++i) {
                if (IsBump(aSlab, i)) {
                    var bumpLength = CalculateBumpLength(aSlab, i);
                    FilterBumping(aSlab, i, bumpLength);
                }
            }                                    
        }

        private static void FilterBumping(SlabModelImpl aSlab, int aStart, int aBumpLength)
        {
            var topLine = aSlab.TopLines[aSlab.TopLines.Length / 2];            
            if (aStart + aBumpLength >= topLine.Length) {
                return;
            }

            var topDifference = topLine[aStart].Y - topLine[aStart + aBumpLength].Y;
            var topLineFirstPointsY = topLine[aStart].Y;
            for (var i = 0; i < aSlab.TopLines.Length; ++i) {
                for (var j = aStart; j < aSlab.TopLines[i].Length; ++j) {
                    if (j <= aStart + aBumpLength) {
                        aSlab.TopLines[i][j].Y = topLineFirstPointsY;
                    }
                    else {
                        aSlab.TopLines[i][j].Y += topDifference;
                    }                    
                }
            }

            var bottomLine = aSlab.BottomLines[aSlab.BottomLines.Length / 2];
            if (aStart + aBumpLength >= bottomLine.Length) {
                return;
            }
            var bottomDifference = bottomLine[aStart].Y - bottomLine[aStart + aBumpLength].Y;
            var bottomLineFirstPointsY = bottomLine[aStart].Y;
            for (var i = 0; i < aSlab.BottomLines.Length; ++i) {
                for (var j = aStart; j < aSlab.BottomLines[i].Length; ++j) {
                    if (j <= aStart + aBumpLength) {
                        aSlab.BottomLines[i][j].Y = bottomLineFirstPointsY;
                    }
                    else {
                        aSlab.BottomLines[i][j].Y += bottomDifference;
                    }
                }
            }
        }

        private static int CalculateBumpLength(SlabModelImpl aSlab, int aStart)
        {
            var topLine = aSlab.TopLines[aSlab.TopLines.Length / 2];
            var bottomLine = aSlab.BottomLines[aSlab.BottomLines.Length / 2];
            for (var i = aStart; i < topLine.Length - 1 && i < bottomLine.Length - 1; ++i) {
                var topDifference = topLine[i].Y - topLine[i + 1].Y;
                var bottomDifference = bottomLine[i].Y - bottomLine[i + 1].Y;
                if (topDifference * bottomDifference <= double.Epsilon) { // имеют разные направления или маленькие                    
                    return i - aStart;
                }
            }

            return topLine.Length - 1 - aStart;
        }

        private static bool IsBump(SlabModelImpl aSlab, int aStart)
        {
            var topLine = aSlab.TopLines[aSlab.TopLines.Length/2];
            var bottomLine = aSlab.BottomLines[aSlab.BottomLines.Length/2];
            for (var i = aStart; 
                i < aStart + BUMP_POINTS_LENGTH && i < topLine.Length - 1 && i < bottomLine.Length - 1; 
                ++i) {
                var topDifference = topLine[i].Y - topLine[i + 1].Y;
                var bottomDifference = bottomLine[i].Y - bottomLine[i + 1].Y;
                if (topDifference*bottomDifference <= double.Epsilon) {// имеют разные направления                    
                    return false;
                }
            }

            return true;
        }
    }
}
