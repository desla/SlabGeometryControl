namespace Alvasoft.DataWriter
{
    /// <summary>
    /// Правило.
    /// </summary>
    public interface IRegulation
    {
        /// <summary>
        /// Возвращает идентификатор.
        /// </summary>
        /// <returns>Идентификатор.</returns>
        int GetId();

        /// <summary>
        /// Возвращает идентификатор измерения.
        /// </summary>
        /// <returns>Идентификатор измерения.</returns>
        int GetDimentionId();

        /// <summary>
        /// Возвращает идентификатор типа-размера.
        /// </summary>
        /// <returns>Идентификатор типа-размера.</returns>
        int GetStandartSizeId();

        /// <summary>
        /// Возвращает максимальное значение.
        /// </summary>
        /// <returns>Максимальное значение.</returns>
        double GetMaxValue();

        /// <summary>
        /// Возвращает минимальное значение.
        /// </summary>
        /// <returns>Минимальное значение.</returns>
        double GetMinValue();
    }
}
