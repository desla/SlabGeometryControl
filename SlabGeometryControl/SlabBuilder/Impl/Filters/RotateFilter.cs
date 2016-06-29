using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alvasoft.SlabBuilder.Impl.Filters
{
    using Utils.Mathematic3D;

    public class RotateFilter
    {        
        public static void Filter(SlabModelImpl aSlab)
        {
            if (aSlab == null) {
                return;
            }

            var linesCount = aSlab.LeftLines.Length;
            for (var i = 0; i < linesCount; ++i) {
                if (i >= aSlab.RightLines.Length) {
                    return;
                }

                var leftLine = aSlab.LeftLines[i];
                var rightLine = aSlab.RightLines[i];

                FilterRotate(leftLine, rightLine);
            }
        }

        private static void FilterRotate(Point3D[] aLeftLine, Point3D[] aRightLine)
        {
            if (aLeftLine == null || aRightLine == null) {
                return;
            }

            var leftLineIndex = 0;
            var rightLineIndex = 0;
            while (leftLineIndex < aLeftLine.Length && 
                   rightLineIndex < aRightLine.Length) {
                if (Math.Abs(aLeftLine[leftLineIndex].Z - aRightLine[rightLineIndex].Z) <= double.Epsilon) {
                    break;
                }

                if (aLeftLine[leftLineIndex].Z < aRightLine[rightLineIndex].Z) {
                    leftLineIndex++;
                }
                else {
                    rightLineIndex++;
                }
            }

            while (leftLineIndex < aLeftLine.Length &&
                   rightLineIndex < aRightLine.Length) {
                if (leftLineIndex == 0 || rightLineIndex == 0) {                    
                    leftLineIndex++;
                    rightLineIndex++;                    
                    continue;                    
                }

                var leftPoint = aLeftLine[leftLineIndex];
                var rightPoint = aRightLine[rightLineIndex];
                var prevLeftPoint = aLeftLine[leftLineIndex - 1];
                var prevRightPoint = aRightLine[rightLineIndex - 1];

                var leftDifference = prevLeftPoint.X - leftPoint.X;
                var rightDifference = prevRightPoint.X - rightPoint.X;

                if (leftDifference*rightDifference > 0) { // если одно направление.
                    // находим минимальное по модулю смещение.
                    var minDifference = Math.Min(Math.Abs(leftDifference), Math.Abs(rightDifference));
                    if (leftDifference < 0) {
                        minDifference *= -1;
                    }
                    for (var i = leftLineIndex; i < aLeftLine.Length; ++i) {
                        aLeftLine[i].X -= minDifference;
                    }
                    for (var i = rightLineIndex; i < aRightLine.Length; ++i) {
                        aRightLine[i].X += minDifference;
                    }
                }

                leftLineIndex++;
                rightLineIndex++;
            }
        }
    }
}
