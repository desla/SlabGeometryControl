using Alvasoft.SensorValueContainer;

namespace Alvasoft.DataWriter
{
    /// <summary>
    /// Писатель значений показаний датчиков.
    /// </summary>
    public interface ISensorValueWriter
    {
        /// <summary>
        /// Записывает паказания датчиков.
        /// </summary>
        /// <param name="aValue">Значение.</param>
        void WriteSensorValueInfo(ISensorValueInfo aValue);
    }
}
