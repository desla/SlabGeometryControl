namespace Emulator.Configuration
{
    using System.Drawing;

    public class SensorConfiguration
    {
        /// <summary>
        /// Положение.
        /// </summary>
        public PointF Position { get; set; }

        /// <summary>
        /// Вектор сканирования.
        /// </summary>
        public PointF ScanVertor { get; set; }
    }
}
