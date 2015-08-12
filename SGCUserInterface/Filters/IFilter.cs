using System.Drawing;
using Alvasoft.SlabGeometryControl;

namespace SGCUserInterface.Filters
{
    public interface IFilter
    {
        PointF[] Filter(PointF[] aPoints);
    }
}
