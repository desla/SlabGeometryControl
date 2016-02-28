namespace Alvasoft.DimentionValueContainer
{
    /// <summary>
    /// Значение параметра слитка.
    /// </summary>
    public interface IDimentionValue
    {
        /// <summary>
        /// Возвращает идентификатор изменения.
        /// </summary>
        /// <returns>Идентификатор измерения.</returns>
        int GetDimentionId();

        /// <summary>
        /// Возвращает значение параметра слитка.
        /// </summary>
        /// <returns>Значение параметра слитка.</returns>
        double GetValue();

        /// <summary>
        /// Возвращает идентификатор слитка.
        /// </summary>
        /// <returns>Идентификатор слитка.</returns>
        int GetSlabId();
    }
}
