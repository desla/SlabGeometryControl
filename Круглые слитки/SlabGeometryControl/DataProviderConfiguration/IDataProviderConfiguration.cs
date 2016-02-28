namespace Alvasoft.DataProviderConfiguration
{
    /// <summary>
    /// Конфигурация поставщика данных.    
    /// </summary>
    public interface IDataProviderConfiguration
    {
        /// <summary>
        /// Возвращает адрес opc-сервера.
        /// </summary>
        /// <returns>адрес opc-сервера.</returns>
        string GetHost();

        /// <summary>
        /// Возвращает имя орс-сервера.
        /// </summary>
        /// <returns>имя орс-сервера.</returns>
        string GetServer();

        /// <summary>
        /// Возвращает описание конторльного блока OPC.
        /// </summary>
        /// <returns>Описание контрольного блока OPC.</returns>
        IOpcControlBlock GetControlBlock();

        /// <summary>
        /// Возвращает количество описаний датчиков OPC.
        /// </summary>
        /// <returns>Количество описаний датчиков OPC.</returns>
        int GetOpcSensorInfoCount();        

        /// <summary>
        /// Добавляет новое описание ОРС-датчика.
        /// </summary>
        /// <param name="aOpcSensorInfo">Описание орс-датчика.</param>
        void CreateOpcSensorInfo(IOpcSensorInfo aOpcSensorInfo);

        /// <summary>
        /// Возвращает описание OPC-датчика по его идентификатору датчика.
        /// </summary>
        /// <param name="aId">Идентификатор датчика.</param>
        /// <returns>Описание OPC-датчика.</returns>
        IOpcSensorInfo ReadOpcSensorInfoById(int aId);

        /// <summary>
        /// Возвращает описание OPC-датчика по его индеку датчика.
        /// </summary>
        /// <param name="aIndex">Индекс датчика.</param>
        /// <returns>Описание OPC-датчика.</returns>
        IOpcSensorInfo ReadOpcSensorInfoByIndex(int aIndex);

        /// <summary>
        /// Возвращает описание OPC-датчика по имени датчика.
        /// </summary>
        /// <param name="aSensorName">Имя датчика.</param>
        /// <returns>Описание OPC-датчика.</returns>
        IOpcSensorInfo ReadOpcSensorInfoByName(string aSensorName);        

        /// <summary>
        /// Обновляет описание орс-датчика с таким же идентификатором.
        /// </summary>
        /// <param name="aOpcSensorInfo">Новое описание орс-датчика.</param>
        void UpdateOpcSensorInfo(IOpcSensorInfo aOpcSensorInfo);

        /// <summary>
        /// Удаляет описание орс-датчика.
        /// </summary>
        /// <param name="aOpcSensorInfo">Описание орс-датчика.</param>
        void DeleteOpcSensorInfo(IOpcSensorInfo aOpcSensorInfo);

        /// <summary>
        /// Подписывает слушателя на изменение конфигурации.
        /// </summary>
        /// <param name="aListener">Слушатель.</param>
        void SubscribeConfigurationListener(IDataProviderConfigurationListener aListener);

        /// <summary>
        /// Удаляет слушателя на изменение конфигурации.
        /// </summary>
        /// <param name="aListener">Слушатель.</param>
        void UnsubscribeConfigurationListener(IDataProviderConfigurationListener aListener);
    }
}
