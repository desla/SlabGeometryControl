namespace Alvasoft.DataProviderConfiguration
{
    /// <summary>
    /// Описание орс-датчика. Будет дополняться
    /// новыми тегами орс.
    /// </summary>
    public interface IOpcSensorInfo
    {
        /// <summary>
        /// Возвращает идентификатор орс-датчика.
        /// </summary>
        /// <returns>Идентификатор орс-датчика.</returns>
        int GetId();

        /// <summary>
        /// Возвращает имя датчика.
        /// </summary>
        /// <returns>Имя датчика.</returns>
        string GetSensorName();
    }
}
