namespace Alvasoft.SensorConfiguration
{
    /// <summary>
    /// Конфигурация датчиков.
    /// </summary>
    public interface ISensorConfiguration
    {
        /// <summary>
        /// Возвращает количество датчиков в конфигурации.
        /// </summary>
        /// <returns>количество датчиков.</returns>
        int GetSensorInfoCount();

        /// <summary>
        /// Добавляет описание датчика.
        /// </summary>
        /// <param name="aSensorInfo">Описание датчика.</param>
        void CreateSensorInfo(ISensorInfo aSensorInfo);

        /// <summary>
        /// Возвращает описание датчика по имени.
        /// </summary>
        /// <param name="aName">Имя датчика.</param>
        /// <returns>Описание датчика.</returns>
        ISensorInfo ReadSensorInfoByName(string aName);

        /// <summary>
        /// Возвращает описание датчика по идентификатору.
        /// </summary>
        /// <param name="aId">Идентификатор.</param>
        /// <returns>Описание датчика.</returns>
        ISensorInfo ReadSensorInfoById(int aId);

        /// <summary>
        /// Возвращает описание датчика по индексу.
        /// </summary>
        /// <param name="aIndex">Индекс.</param>
        /// <returns>Описание датчика.</returns>
        ISensorInfo ReadSensorInfoByIndex(int aIndex);

        /// <summary>
        /// Обновляет информацию для датчика с таким же идентификатором.
        /// </summary>
        /// <param name="aSensorInfo">Описание датчика.</param>
        void UpdateSensorInfo(ISensorInfo aSensorInfo);

        /// <summary>
        /// Удаляет описание датчика из конфигурации.
        /// </summary>
        /// <param name="aSensorInfo">Описание датчика.</param>
        void DeleteSensorInfo(ISensorInfo aSensorInfo);

        /// <summary>
        /// Подписывает слушателя на изменение конфигурации.
        /// </summary>
        /// <param name="aListener">Слушатель.</param>
        void SubscribeConfigurationListener(ISensorConfigurationListener aListener);

        /// <summary>
        /// Удаляет слушателя на изменение конфигурации.
        /// </summary>
        /// <param name="aListener">Слушатель.</param>
        void UnsubscribeConfigurationListener(ISensorConfigurationListener aListener);
    }
}
