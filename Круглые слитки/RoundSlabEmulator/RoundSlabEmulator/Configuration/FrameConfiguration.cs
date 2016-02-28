namespace REmulatorConfiguration {
    /// <summary>
    /// Конфигурация рамки.
    /// </summary>
    public class FrameConfiguration {
        /// <summary>
        /// Конфигурация датчиков.
        /// </summary>
        public SensorConfiguration[] Sensors { get; set; }

        /// <summary>
        /// Частота сканирования, раз в мм.
        /// </summary>
        public double ScanSpeed { get; set; }

        /// <summary>
        /// Погрешность.
        /// </summary>
        public double Error { get; set; }
    }
}
