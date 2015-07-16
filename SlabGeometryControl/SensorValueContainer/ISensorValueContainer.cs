using Alvasoft.SensorConfiguration;

namespace Alvasoft.SensorValueContainer
{
    /// <summary>
    /// Накопитель данных с датчиков.
    /// </summary>
    public interface ISensorValueContainer
    {
        /// <summary>
        /// Добавляет значение датчика.
        /// </summary>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <param name="aValue">Значение.</param>
        /// <param name="aTime">Время.</param>
        void AddSensorValue(int aSensorId, double aValue, long aTime);

        /// <summary>
        /// Возвращает значения датчиков, собранные в накопителе после указанонго времени.
        /// </summary>
        /// <param name="aTime">Время.</param>
        /// <returns>Значения датчиков.</returns>
        ISensorValueInfo[] GetAllValues(long aTime);

        /// <summary>
        /// Возвращает значения датчиков по номеру датчика.
        /// </summary>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>Значения датчика.</returns>
        ISensorValueInfo[] GetSensorValuesBySensorId(int aSensorId);

        /// <summary>
        /// Очищает накопитель.
        /// </summary>
        void Clear();

        /// <summary>
        /// Возвращает признак пустоты накопителя.
        /// </summary>
        /// <returns>true, если данных нет, false - иначе.</returns>
        bool IsEmpty();

        /// <summary>
        /// Добавляет слушателя на добавление данных в накопитель.
        /// </summary>
        /// <param name="aListener">Слушатель.</param>
        void SunbscribeContainerListener(ISensorValueContainerListener aListener);

        /// <summary>
        /// Удаляет слушателя на добавление данных в накопитель.
        /// </summary>
        /// <param name="aListener">Слушатель.</param>
        void UnsunbscribeContainerListener(ISensorValueContainerListener aListener);
    }
}
