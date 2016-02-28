namespace Alvasoft.DataProviderConfiguration
{
    public interface IDataProviderConfigurationListener
    {
        /// <summary>
        /// Возникает при добавлении датчика.
        /// </summary>
        /// <param name="aConfiguration">Конфигурация.</param>
        /// <param name="aSensorId">Идентификатор нового датчика.</param>
        void OnSensorCreated(IDataProviderConfiguration aConfiguration, int aSensorId);

        /// <summary>
        /// Возникает при обновлении датчика.
        /// </summary>
        /// <param name="aConfiguration">Конфигурация.</param>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        void OnSensorUpdated(IDataProviderConfiguration aConfiguration, int aSensorId);

        /// <summary>
        /// Возникает при удалении датчика.
        /// </summary>
        /// <param name="aConfiguration">Конфигурация.</param>
        /// <param name="aSensorId">Идентификатор удаленного датчика.</param>
        void OnSensorDeleted(IDataProviderConfiguration aConfiguration, int aSensorId);
    }
}
