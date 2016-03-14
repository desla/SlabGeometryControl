using Alvasoft.Utils.Activity;

namespace Alvasoft.DataProvider
{
    /// <summary>
    /// Калибратор.
    /// </summary>
    public interface ICalibrator : Initializable
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
        double GetCalibratedValue(int aSensorId);

        /// <summary>
        /// Устанавливает калибровочное значение для датчика.
        /// </summary>
        /// <param name="aSensorId">Идентификатор датчика.</param>
        /// <param name="aCalibratedValue">Калибровочное значение.</param>
        void SetCalibratedValue(int aSensorId, double aCalibratedValue);
    }
}
