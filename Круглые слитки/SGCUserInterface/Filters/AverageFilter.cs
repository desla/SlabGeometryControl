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
        }        
    }
}
