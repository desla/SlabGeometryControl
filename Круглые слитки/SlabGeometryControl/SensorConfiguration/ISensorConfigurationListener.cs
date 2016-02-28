namespace Alvasoft.SensorConfiguration
{
    /// <summary>
    /// Подписчик на изменение конфигурации датчиков.
    /// </summary>
    public interface ISensorConfigurationListener
    {
        /// <summary>
        /// Возникает при добавлении датчика.
        /// </summary>
        /// <param name="aConfiguration">Конфигурация.</param>
        /// <param name="aSensorId">Идентификатор нового датчика.</param>
        void OnSensorCreated(ISensorConfiguration aConfiguration, int aSensorId);

        /// <summary>
        /// Возникает при обновлении датчика.
        /// </summary>
        /// <param name="aConfiguration">Конфигурация.</param>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        void OnSensorUpdated(ISensorConfiguration aConfiguration, int aSensorId);

        /// <summary>
        /// Возникает при удалении датчика.
        /// </summary>
        /// <param name="aConfiguration">Конфигурация.</param>
        /// <param name="aSensorId">Идентификатор удаленного датчика.</param>
        void OnSensorDeleted(ISensorConfiguration aConfiguration, int aSensorId);
    }
}
