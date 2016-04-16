using Alvasoft.DataProvider;
using Alvasoft.SensorConfiguration;
using Alvasoft.SensorValueContainer;

namespace Alvasoft.SlabBuilder
{
    /// <summary>
    /// Построитель моделей слитков.
    /// </summary>
    public interface ISlabBuilder
    {
        /// <summary>
        /// Устанавливает конфигурацию датчиков.
        /// </summary>
        /// <param name="aConfiguration">Конфигурация датчиков.</param>
        void SetSensorConfiguration(ISensorConfiguration aConfiguration);

        /// <summary>
        /// Устанавливает накопитель данных из датчиков.
        /// </summary>
        /// <param name="aContainer"></param>
        void SetSensorValueContainer(ISensorValueContainer aContainer);

        /// <summary>
        /// Устанавливает калибратор.
        /// </summary>
        /// <param name="aCalibrator">Калибратор.</param>
        void SetCalibrator(ICalibrator aCalibrator);

        /// <summary>
        /// Выполняет построение модели слитка.
        /// </summary>
        /// <returns>Модель слитка.</returns>
        ISlabModel BuildSlabModel(bool aIsUseFilters = true);
    }
}
