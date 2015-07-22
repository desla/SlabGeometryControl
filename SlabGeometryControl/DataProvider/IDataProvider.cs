using Alvasoft.DataProviderConfiguration;
using Alvasoft.DataEnums;
using Alvasoft.SensorConfiguration;
using Alvasoft.SensorValueContainer;

namespace Alvasoft.DataProvider
{
    /// <summary>
    /// Предоставляет данные из источника данных.
    /// Источник данных - OPC сервер.
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Возвращает текущее состояние системы.
        /// </summary>
        /// <returns>Состояние системы.</returns>
        SystemState GetCurrentSystemState();

        /// <summary>
        /// Устанавливает накопитель данных с датчиков.
        /// </summary>
        /// <param name="aSensorValueContainer">Накопитель данных датчиков.</param>
        void SetSensorValueContainer(ISensorValueContainer aSensorValueContainer);

        /// <summary>
        /// Устанавливает конфигурация DataProvider'а.
        /// </summary>
        /// <param name="aDataProviderConfiguration">Конфигурация DataProvider'а.</param>
        void SetDataProviderConfiguration(IDataProviderConfiguration aDataProviderConfiguration);

        /// <summary>
        /// Устанавливает конфигурацию датчиков системы.
        /// ! Это не конфигурация DataProvider'а!
        /// </summary>
        /// <param name="aSensorConfiguration">Конфигурация датчиков.</param>
        void SetSensorConfiguration(ISensorConfiguration aSensorConfiguration);

        /// <summary>
        /// Добавляет подписчика на измерение состояния системы.
        /// </summary>
        /// <param name="aListener">Подписчик.</param>
        void SubscribeDataProviderListener(IDataProviderListener aListener);

        /// <summary>
        /// Удалаяет подписчика.
        /// </summary>
        /// <param name="aListener">Подписчик.</param>
        void UnsubscribeDataProviderListener(IDataProviderListener aListener);

        /// <summary>
        /// Возвращает состояние подключения.
        /// </summary>
        /// <returns></returns>
        bool IsConnected();

        /// <summary>
        /// Возвращает состояние датчика.
        /// </summary>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>Состояние работы датчика.</returns>
        bool IsSensorActive(int aSensorId);

        /// <summary>
        /// Возвращает текущее показание датчика.
        /// </summary>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>Показание датчика.</returns>
        double GetSensorCurrentValue(int aSensorId);
    }
}
