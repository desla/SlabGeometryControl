using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface.Filters
{
    public class AverageFilter
    {
        private const int WINDOW_SIZE = 10; // размер окна для вычисления среднего.

        public static void Filter(SlabModel3D aSlab)
        {
            if (aSlab == null) {
                return;
            }
            
            for (var i = 0; i < aSlab.TopLines.Length; ++i) {
                var segments = FindSegments(aSlab.TopLines[i]);                
                FilterLine(aSlab.TopLines[i], segments);                
            }

            for (var i = 0; i < aSlab.BottomLines.Length; ++i) {
                var segments = FindSegments(aSlab.BottomLines[i]);
                FilterLine(aSlab.BottomLines[i], segments);                
            }
        }

        private static int[] FindSegments(SlabPoint[] aLine)
        {
            var segments = new List<int>();
            for (var i = 0; i < aLine.Length; ++i) {
                var difLeft = false;
                var difRight = false;
                if (i == 0 || Math.Abs(DxDivDy(aLine[i], aLine[i - 1])) >= 1) {
                    difLeft = true;
                }

                if (i == aLine.Length - 1 || Math.Abs(DxDivDy(aLine[i], aLine[i + 1])) >= 1) {
                    difRight = true;
                }

                if (difRight ^ difLeft) {
                    segments.Add(i);
                }
            }

            return segments.ToArray();
        }

        private static double DxDivDy(SlabPoint aPointA, SlabPoint aPointB)
        {
            return (aPointA.Y - aPointB.Y)/(aPointA.Z - aPointB.Z);
        }

        private static void FilterLine(SlabPoint[] aLine, int[] aSegments)
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
