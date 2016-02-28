namespace REmulatorConfiguration {
    using System.Drawing;

    /// <summary>
    /// Конфигурация подпрыгивания.
    /// </summary>
    public class BumpConfiguration {
        /// <summary>
        /// Позиция на слитке.
        /// </summary>
        public double Position { get; set; }

        /// <summary>
        /// Направление.
        /// </summary>
        public PointF Direction { get; set; }
    }
}
