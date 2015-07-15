namespace Alvasoft.SensorValueContainer
{
    /// <summary>
    /// Описание данных от датчика.
    /// </summary>
    public interface ISensorValueInfo
    {
        /// <summary>
        /// Возвращает идентификатор датчика.
        /// </summary>
        /// <returns>Идентификатор датчика.</returns>
        int GetSensorId();

        /// <summary>
        /// Возвращает показание датчика.
        /// </summary>
        /// <returns>Показание датчика.</returns>
        double GetValue();

        /// <summary>
        /// Возвращает время снятия показания датчика.
        /// </summary>
        /// <returns>Время.</returns>
        long GetTime();
    }
}
