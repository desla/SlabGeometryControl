namespace REmulatorConfiguration {
    using System.Drawing;

    public class SensorConfiguration {

        /// <summary>
        /// Идентификатор датчика.
        /// </summary>
        public int Id { get; set; }

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
