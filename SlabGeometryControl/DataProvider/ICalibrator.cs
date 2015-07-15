namespace Alvasoft.DataProvider
{
    /// <summary>
    /// Калибратор.
    /// </summary>
    public interface ICalibrator
    {
        /// <summary>
        /// Возвращает можно ли калиброваться в настоящее время.
        /// </summary>
        /// <returns>True, если калиброваться можно, false - иначе.</returns>
        bool IsCalibratedState();

        /// <summary>
        /// Возвращает калибровочное значение для указанного датчика.
        /// </summary>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <returns>Калибровочное значение.</returns>
        double GetCalibratedValueBySensorId(int aSensorId);
    }
}
