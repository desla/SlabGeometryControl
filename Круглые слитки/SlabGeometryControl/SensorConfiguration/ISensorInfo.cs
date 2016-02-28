using Alvasoft.DataEnums;

namespace Alvasoft.SensorConfiguration
{
    /// <summary>
    /// Описание датчика.
    /// </summary>
    public interface ISensorInfo
    {
        /// <summary>
        /// Возвращает идентификатор датчика.
        /// </summary>
        /// <returns>Идентификатор датчика.</returns>
        int GetId();

        /// <summary>
        /// Возвращает имя датчика.
        /// </summary>
        /// <returns>Имя датчика.</returns>
        string GetName();

        /// <summary>
        /// Возвращает тип датчика.
        /// </summary>
        /// <returns>Тип датчика.</returns>
        SensorType GetSensorType();

        /// <summary>
        /// Возвращает сторону датчика.
        /// </summary>
        /// <returns>Сторона датчика.</returns>
        SensorSide GetSensorSide();

        /// <summary>
        /// Возвращает смещение датчика относительно середины стороны рамки.
        /// </summary>
        /// <returns>Смещение относительно середины стороны.</returns>
        double GetShift();
    }
}
