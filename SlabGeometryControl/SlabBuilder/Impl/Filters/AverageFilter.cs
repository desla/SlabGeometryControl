using System;
using System.Collections.Generic;
using Alvasoft.Utils.Mathematic3D;

namespace Alvasoft.SlabBuilder.Impl.Filters
{
    public class AverageFilter
    {
        private const int WINDOW_SIZE = 10; // размер окна для вычисления среднего.
        private const double MIN_SPEED = 0.5; // минимальное значение производной.

        public static void Filter(SlabModelImpl aSlab)
        {
            if (aSlab == null) {
                return;
            }
            
            for (var i = 0; i < aSlab.TopLines.Length; ++i) {
                var segments = FindVerticalSegments(aSlab.TopLines[i]);                
                FilterVerticalLine(aSlab.TopLines[i], segments);                
            }

            for (var i = 0; i < aSlab.BottomLines.Length; ++i) {
                var segments = FindVerticalSegments(aSlab.BottomLines[i]);
                FilterVerticalLine(aSlab.BottomLines[i], segments);                
            }

            for (var i = 0; i < aSlab.LeftLines.Length; ++i) {
                var segments = FindHorizontalSegments(aSlab.LeftLines[i]);
                FilterHorizontLine(aSlab.LeftLines[i], segments);
            }

            for (var i = 0; i < aSlab.RightLines.Length; ++i) {
                var segments = FindHorizontalSegments(aSlab.RightLines[i]);
                FilterHorizontLine(aSlab.RightLines[i], segments);
            }
        }

        private static int[] FindHorizontalSegments(Point3D[] aLine)
        {
            var segments = new List<int>();
            for (var i = 0; i < aLine.Length; ++i) {
                var difLeft = false;
                var difRight = false;
                if (i == 0 || Math.Abs(DxDivDz(aLine[i], aLine[i - 1])) >= MIN_SPEED) {
                    difLeft = true;
                }

                if (i == aLine.Length - 1 || Math.Abs(DxDivDz(aLine[i], aLine[i + 1])) >= MIN_SPEED) {
                    difRight = true;
                }

                if (difRight ^ difLeft) {
                    segments.Add(i);
                }
            }

            return segments.ToArray();
        }

        private static void FilterHorizontLine(Point3D[] aLine, int[] aSegments)
        {
            if (aLine == null) {
                return;
            }

            for (var s = 0; s < aSegments.Length / 2; ++s) {
                var from = aSegments[s * 2];
                var to = aSegments[s * 2 + 1];
                if (to - from >= WINDOW_SIZE * 2 + 1) {
                    var averages = new double[to - from + 1];
                    var index = 0;
                    for (var i = from; i <= to; ++i) {
                        var summed = 0.0;
                        var count = 0;
                        for (var j = i - WINDOW_SIZE; j <= i + WINDOW_SIZE; ++j) {
                            if (j >= 0 && j < aLine.Length && j >= from && j <= to) {
                                summed += aLine[j].X;
                                count++;
                            }
                        }
                        averages[index++] = summed / count;
                    }

                    index = 0;
                    for (var i = from; i <= to; ++i) {
                        aLine[i].X = averages[index++];
                    }
                }
            }
        }

        private static int[] FindVerticalSegments(Point3D[] aLine)
        {
            var segments = new List<int>();
            for (var i = 0; i < aLine.Length; ++i) {
                var difLeft = false;
                var difRight = false;
                if (i == 0 || Math.Abs(DyDivDz(aLine[i], aLine[i - 1])) >= MIN_SPEED) {
                    difLeft = true;
                }

                if (i == aLine.Length - 1 || Math.Abs(DyDivDz(aLine[i], aLine[i + 1])) >= MIN_SPEED) {
                    difRight = true;
                }

                if (difRight ^ difLeft) {
                    segments.Add(i);
                }
            }

            return segments.ToArray();
        }

        private static double DyDivDz(Point3D aPointA, Point3D aPointB)
        {
            return (aPointA.Y - aPointB.Y)/(aPointA.Z - aPointB.Z);
        }

        private static double DxDivDz(Point3D aPointA, Point3D aPointB)
        {
            return (aPointA.X - aPointB.X) / (aPointA.Z - aPointB.Z);
        }

        private static void FilterVerticalLine(Point3D[] aLine, int[] aSegments)
        {
            if (aLine == null) {
                return;
            }

            for (var s = 0; s < aSegments.Length/2; ++s) {
                var from = aSegments[s*2];
                var to = aSegments[s*2 + 1];
                if (to - from >= WINDOW_SIZE*2 + 1) {
                    var averages = new double[to - from + 1];
                    var index = 0;
                    for (var i = from; i <= to; ++i) {
                        var summed = 0.0;
                        var count = 0;
                        for (var j = i - WINDOW_SIZE; j <= i + WINDOW_SIZE; ++j) {
                            if (j >= 0 && j < aLine.Length && j >= from && j <= to) {
                                summed += aLine[j].Y;
                                count++;
                            }
                        }
                        averages[index++] = summed / count;
                    }

                    index = 0;
                    for (var i = from; i <= to; ++i) {
                        aLine[i].Y = averages[index++];
                    }
                }                
            }            
        }
    }
}
